using Blazui.Community.Response;
using Blazui.Community.StringExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Blazui.Community.Api
{
    /// <summary>
    ///
    /// </summary>
    public class LoggerMiddleware
    {
        private static ILogger<LoggerMiddleware> _logger;

        private readonly RequestDelegate _next;

        /// <summary>
        /// 计时器
        /// </summary>
        private Stopwatch _stopwatch;

        /// <summary>
        ///
        /// </summary>
        /// <param name="next"></param>
        public LoggerMiddleware(RequestDelegate next, ILogger<LoggerMiddleware> logger)
        {
            _logger = logger;
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
            if (request.Path.ToString().IfContains("upload"))
            {
                await _next(context);
            }
            else
            {
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
                    WriteLog(model);
                    return Task.CompletedTask;
                });
            }
        }

        private static void WriteLog(HttpMiddlewareModel model)
        {
            if (!model.Request.Url.IfContains("swagger") && !model.Request.Url.IfContains("html"))
            {
                _logger.LogDebug("============================================================================");
                _logger.LogDebug($"Start：{model.ExecuteStart}");
                _logger.LogDebug($"Url：{model.Request.Url}");
                _logger.LogDebug($"Header：");
                _logger.LogDebug($"{string.Join("\r\n                          ", model.Request.Header.ToArray())}");
                _logger.LogDebug($"Request：{model.Request.Param}");
                _logger.LogDebug($"Response：");
                _logger.LogDebug($"{JsonConvert.SerializeObject(model.Response)}");
                _logger.LogDebug($"End：{model.ExecuteEnd}");
                _logger.LogDebug($"ElapsedTime：{model.ElapsedTime}");
                _logger.LogDebug("============================================================================");
            }
        }

        /// <summary>
        /// 获取响应内容
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private async Task<string> GetResponse(HttpResponse response)
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
            await response.WriteAsync(JsonConvert.SerializeObject(new BadRequestResponse(exception.Message)))
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