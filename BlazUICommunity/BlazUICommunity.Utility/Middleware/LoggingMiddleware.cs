using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using System.IO.Compression;
using BlazUICommunity.Utility;
using Microsoft.AspNetCore.Mvc;

namespace BlazUICommunity.Utility.MiddleWare
{
    /// <summary>
    /// 
    /// </summary>
    public class LoggerMiddleware
    {
        private static   ILogger<LoggerMiddleware> _logger;
        public LoggerMiddleware(ILogger<LoggerMiddleware> logger)
        {
            _logger = logger;
        }
        private readonly RequestDelegate _next;
        /// <summary>
        /// 计时器
        /// </summary>
        private Stopwatch _stopwatch;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public LoggerMiddleware(RequestDelegate next)
        {
            _next = next;
            _stopwatch = new Stopwatch();
        }
        /// <summary>
        /// 拦截请求参数打印日志
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            _stopwatch.Restart();
            HttpRequest request = context.Request;
            HttpMiddlewareModel model = new HttpMiddlewareModel() { };
            model.Request = new RequestBody();
            model.ExecuteStart = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            model.Request.Url = request.Path.ToString();
            model.Request.Header = request.Headers.ToDictionary(x => x.Key, v => string.Join(";", v.Value.ToList()));
            model.Request.Method = request.Method;

            try
            {
                // 获取请求body内容
                if (request.Method.ToLower().Equals("post"))
                {
                    // 启用倒带功能，就可以让 Request.Body 可以再次读取
                    request.EnableBuffering();
                    Stream stream = request.Body;
                    byte[] buffer = new byte[request.ContentLength.Value];
                    await stream.ReadAsync(buffer, 0, buffer.Length);
                    model.Request.Param = Encoding.Default.GetString(buffer);
                    request.Body.Position = 0;
                }
                else if (request.Method.ToLower().Equals("get"))
                {
                    model.Request.Param = request.QueryString.Value;
                }


                //// 获取Response.Body内容
                var originalBodyStream = context.Response.Body;
                using var responseBody = new MemoryStream();
                context.Response.Body = responseBody;
                try
                {
                    await _next(context);
                }
                catch (Exception ex)
                {
                    await HandleExceptionAsync(context, ex);
                }
                model.Response = await GetResponse(context.Response);
                model.ExecuteEnd = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                await responseBody.CopyToAsync(originalBodyStream);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
            // 响应完成记录时间和存入日志
            context.Response.OnCompleted(() =>
                    {
                        _stopwatch.Stop();
                        model.ElapsedTime = $"{_stopwatch.ElapsedMilliseconds }ms";
                        if (!model.Request.Url.ToLower().Contains("swagger") || model.Request.Url.ToLower().Contains("html"))
                            _logger.LogDebug($"request->response：\r\n{JsonConvert.SerializeObject(model)}");
                        return Task.CompletedTask;
                    });
        }
        /// <summary>
        /// 获取响应内容
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public async Task<string> GetResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return text;
        }


        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (exception == null) return;
            await ExceptionReponseAsync(context, exception).ConfigureAwait(false);
        }

        private async Task ExceptionReponseAsync(HttpContext context, Exception exception)
        {
            //记录日志
            _logger.LogError($"path={context.Request.Path}\r\n{exception.StackTrace}");
            //返回友好的提示
            var response = context.Response;
            //状态码
            if (exception is UnauthorizedAccessException)
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
            else if (exception is Exception)
                response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.ContentType = context.Request.Headers["Accept"];
            response.ContentType = "application/json";
            await response.WriteAsync(JsonConvert.SerializeObject(new BadRequestResult()))
                .ConfigureAwait(false);

        }


    }


    /// <summary>
    /// 
    /// </summary>
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseLogMiddleware(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            return app.UseMiddleware<LoggerMiddleware>();
        }
    }


    public class HttpMiddlewareModel
    {
        public string ExecuteStart { get; set; }
        public string ExecuteEnd { get; set; }
        public string ElapsedTime { get; set; }
        public RequestBody Request { get; set; } = new RequestBody() { Param = "", Method = "", Url = "" };
        public string Response { get; set; }
    }
    public class RequestBody
    {

        public string Url { get; set; }
        public Dictionary<string, string> Header { get; set; }
        public string Method { get; set; }
        public string Param { get; set; }

    }
}
