using Blazui.Community.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Blazui.Community.HttpClientExtensions
{
    public static class HttpClientExtension
    {
        private static readonly Dictionary<string, string> EtagCaches = new Dictionary<string, string>();
        private static readonly string IfNoMatch = "If-None-Match";

        public static async Task<BaseResponse<T>> GetWithJsonResultAsync<T>
        (this HttpClient httpClient, string url)
     => await httpClient.GetJsonResultAsync<T>(url, HttpMethod.Get);

        public static async Task<BaseResponse<T>> DeleteWithJsonResultAsync<T>
         (this HttpClient httpClient, string url)
         => await httpClient.GetJsonResultAsync<T>(url, HttpMethod.Delete);

        public static async Task<BaseResponse<T>> PatchWithJsonResultAsync<T>
          (this HttpClient httpClient, string url, HttpContent httpContent = null)
          => await httpClient.GetJsonResultAsync<T>(url, HttpMethod.Patch, httpContent);

        public static async Task<BaseResponse<T>> PutWithJsonResultAsync<T>
            (this HttpClient httpClient, string url, HttpContent httpContent = null)
            => await httpClient.GetJsonResultAsync<T>(url, HttpMethod.Put, httpContent);

        public static async Task<BaseResponse<T>> PostWithJsonResultAsync<T>
        (this HttpClient httpClient, string url, HttpContent httpContent = null)
        => await httpClient.GetJsonResultAsync<T>(url, HttpMethod.Post, httpContent);

        public static async Task<BaseResponse> GetWithJsonResultAsync
      (this HttpClient httpClient, string url)
   => await httpClient.GetJsonResultAsync(url, HttpMethod.Get);

        public static async Task<BaseResponse> DeleteWithJsonResultAsync
         (this HttpClient httpClient, string url)
         => await httpClient.GetJsonResultAsync(url, HttpMethod.Delete);

        public static async Task<BaseResponse> PatchWithJsonResultAsync
          (this HttpClient httpClient, string url, HttpContent httpContent = null)
          => await httpClient.GetJsonResultAsync(url, HttpMethod.Patch, httpContent);

        public static async Task<BaseResponse> PutWithJsonResultAsync
            (this HttpClient httpClient, string url, HttpContent httpContent = null)
            => await httpClient.GetJsonResultAsync(url, HttpMethod.Put, httpContent);

        public static async Task<BaseResponse> PostWithJsonResultAsync
        (this HttpClient httpClient, string url, HttpContent httpContent = null)
        => await httpClient.GetJsonResultAsync(url, HttpMethod.Post, httpContent);

        /// <summary>
        /// 发送请求返回BaseResponse<T>结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpClient"></param>
        /// <param name="url"></param>
        /// <param name="httpMethod"></param>
        /// <param name="httpContent"></param>
        /// <returns></returns>
        public static async Task<BaseResponse<T>> GetJsonResultAsync<T>(this HttpClient httpClient, string url, HttpMethod httpMethod = HttpMethod.Get, HttpContent httpContent = null)
        {
            HttpResponseMessage response = null;
            try
            {
                response = await httpClient.GetHttpResponseMessage(url, httpMethod, httpContent);
                return response.StatusCode == System.Net.HttpStatusCode.NotModified || response.StatusCode == System.Net.HttpStatusCode.NoContent ?
                    new BaseResponse<T>((int)response.StatusCode) :
                 JsonConvert.DeserializeObject<BaseResponse<T>>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                return response == null ? new BaseResponse<T>(400, ex.Message) : new BaseResponse<T>((int)response.StatusCode, ex.Message);
            }
        }

        /// <summary>
        /// 发送请求返回BaseResponse结果
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="url"></param>
        /// <param name="httpMethod"></param>
        /// <param name="httpContent"></param>
        /// <returns></returns>
        public static async Task<BaseResponse> GetJsonResultAsync(this HttpClient httpClient, string url, HttpMethod httpMethod = HttpMethod.Get, HttpContent httpContent = null)
        {
            HttpResponseMessage response = null;
            try
            {
                response = await httpClient.GetHttpResponseMessage(url, httpMethod, httpContent);
                return response.StatusCode == System.Net.HttpStatusCode.NotModified ?
                     new BaseResponse((int)response.StatusCode) :
                  JsonConvert.DeserializeObject<BaseResponse>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                return response == null ? new BaseResponse(400, ex.Message) : new BaseResponse((int)response.StatusCode, ex.Message);
            }
        }

        private static async Task<HttpResponseMessage> GetHttpResponseMessage(this HttpClient httpClient, string url, HttpMethod httpMethod, HttpContent httpContent)
        {
            if (httpClient is null)
                throw new ArgumentNullException(nameof(httpClient));

            if (string.IsNullOrEmpty(url))
                throw new ArgumentException(nameof(url));

            httpContent ??= new StringContent("");

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //if ((httpMethod == HttpMethod.Post || httpMethod == HttpMethod.Put) && httpContent is null)
            //    throw new ArgumentException(nameof(httpContent));

            HttpResponseMessage response;
            switch (httpMethod)
            {
                case HttpMethod.Get:
                    var ETagCacheKey = url.Contains("?") ? url.Substring(0, url.IndexOf('?')) : url;
                    if (EtagCaches.TryGetValue(ETagCacheKey, out string OldEtag))
                        httpClient.AddEtagToHttpHeader(OldEtag);

                    response = await httpClient.GetAsync(url);
                    var CurrentETag = response.Headers.GetValues("ETag")?.First();
                    OldEtag ??= string.Empty;
                    if (!string.IsNullOrWhiteSpace(CurrentETag) && !OldEtag.Equals(CurrentETag))
                        CreateOrResetEtagCache(CurrentETag, ETagCacheKey);
                    break;

                case HttpMethod.Post:
                    response = await httpClient.PostAsync(url, httpContent);
                    break;

                case HttpMethod.Delete:
                    response = await httpClient.DeleteAsync(url);
                    break;

                case HttpMethod.Put:
                    response = await httpClient.PutAsync(url, httpContent);
                    break;

                case HttpMethod.Patch:
                    response = await httpClient.PatchAsync(url, httpContent);
                    break;

                case HttpMethod.Head:
                    response = await httpClient.HeadAsync(url, httpContent);
                    break;

                case HttpMethod.Trace:
                    response = await httpClient.Tracesync(url, httpContent);
                    break;

                case HttpMethod.Options:
                    response = await httpClient.OptionsAsync(url, httpContent);
                    break;

                default:
                    throw new NotSupportedException(nameof(httpMethod));
            }

            return response;
        }

        /// <summary>
        /// 缓存etag
        /// </summary>
        /// <param name="ETag"></param>
        /// <param name="ETagCacheKey"></param>
        private static void CreateOrResetEtagCache(string ETag, string ETagCacheKey)
        {
            if (EtagCaches.ContainsKey(ETagCacheKey))
                EtagCaches.Remove(ETagCacheKey);
            EtagCaches.Add(ETagCacheKey, ETag);
        }

        /// <summary>
        /// 添加etag验证到httpheader
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="etag"></param>
        private static void AddEtagToHttpHeader(this HttpClient httpClient, string etag)
        {
            if (httpClient.DefaultRequestHeaders.Contains(IfNoMatch))
                httpClient.DefaultRequestHeaders.Remove(IfNoMatch);
            httpClient.DefaultRequestHeaders.Add(IfNoMatch, etag);
        }

        private static async Task<HttpResponseMessage> HeadAsync(this HttpClient httpClient, string url, HttpContent httpContent)
        {
            return await httpClient.OtherHttpMethodRequest(url, httpContent, System.Net.Http.HttpMethod.Head);
        }

        private static async Task<HttpResponseMessage> OptionsAsync(this HttpClient httpClient, string url, HttpContent httpContent)
        {
            return await httpClient.OtherHttpMethodRequest(url, httpContent, System.Net.Http.HttpMethod.Options);
        }

        private static async Task<HttpResponseMessage> Tracesync(this HttpClient httpClient, string url, HttpContent httpContent)
        {
            return await httpClient.OtherHttpMethodRequest(url, httpContent, System.Net.Http.HttpMethod.Trace);
        }

        private static async Task<HttpResponseMessage> OtherHttpMethodRequest(this HttpClient httpClient, string url, HttpContent httpContent, System.Net.Http.HttpMethod httpMethod)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage
            {
                Content = httpContent,
                RequestUri = new Uri(url),
                Method = httpMethod
            };
            return await httpClient.SendAsync(httpRequestMessage);
        }
    }

    public enum HttpMethod
    {
        Get,
        Post,
        Delete,
        Put,
        Patch,
        Head,
        Trace,
        Options
    }
}