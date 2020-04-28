using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Blazui.Community.App.Middleware
{
    public class SeoMiddleware
    {
        private readonly ILogger<SeoMiddleware> _logger;
        private readonly RequestDelegate _next;
        private readonly HttpClient httpClient;

        /// <summary>
        ///
        /// </summary>
        /// <param name="next"></param>
        /// <param name="httpClient"></param>
        /// <param name="logger"></param>
        public SeoMiddleware(RequestDelegate next, IHttpClientFactory httpClientFactory, ILogger<SeoMiddleware> logger)
        {
            _logger = logger;
            _next = next;
            this.httpClient = httpClientFactory.CreateClient("BlazuiCommunitiyApp");
        }

        /// <summary>
        /// 拦截请求参数打印日志
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            HttpRequest request = context.Request;
            request.Headers.TryGetValue("User-Agent", out var userAgent);

            if (userAgent.ToString().Contains("Baiduspider"))
            {
                _logger.LogInformation("baidu正在访问");
                //当前是百度访问，不再继续渲染，跳转至百度专用页面
                //为了不让百度真的跳转，内部用HttpClient进行一次访问，访问百度专用页面
                if (!Debugger.IsAttached)
                {
                    if (request.Scheme != "https")
                    {
                        return;
                    }
                }
                //// 获取Response.Body内容
                var paths = request.Path.Value.Split('/').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                if (paths.Length < 2)
                {
                    return;
                }
                if (paths[0] != "topic")
                {
                    return;
                }
                var responseResult = await httpClient.GetAsync($"{request.Scheme}://{request.Host}/topic_seo/{paths[1]}");
                await responseResult.Content.CopyToAsync(context.Response.Body);
                return;
            }
            else
            {
                await _next(context);
            }

        }

        /// <summary>
        /// 获取响应内容
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private async Task<string> GetResponse(Stream response)
        {
            response.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response).ReadToEndAsync();
            response.Seek(0, SeekOrigin.Begin);
            return text;
        }
    }
}
