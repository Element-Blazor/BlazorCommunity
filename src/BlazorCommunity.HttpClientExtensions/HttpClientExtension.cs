using BlazorCommunity.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;

namespace BlazorCommunity.HttpClientExtensions
{
    public static class HttpClientExtension
    {
        private static readonly ConcurrentDictionary<string, string> EtagCaches = new ConcurrentDictionary<string, string>();
        private static ConcurrentDictionary<string, PropertyInfo[]> QuaryParams = new ConcurrentDictionary<string, PropertyInfo[]>();
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
        private static async Task<BaseResponse<T>> GetJsonResultAsync<T>(this HttpClient httpClient, string url, HttpMethod httpMethod, HttpContent httpContent = null)
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
        private static async Task<BaseResponse> GetJsonResultAsync(this HttpClient httpClient, string url, HttpMethod httpMethod, HttpContent httpContent = null)
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

            #region 对Get请求做缓存处理

            //if (httpMethod == HttpMethod.Get)
            //{
            //    var ETagCacheKey = url.Contains("?") ? url.Substring(0, url.IndexOf('?')) : url;
            //    if (EtagCaches.TryGetValue(ETagCacheKey, out string OldEtag))
            //        httpClient.AddEtagToHttpHeader(OldEtag);
            //    var response = await httpClient.GetAsync(url);
            //    var CurrentETag = response.Headers.GetValues("ETag")?.First();
            //    OldEtag ??= string.Empty;
            //    if (!string.IsNullOrWhiteSpace(CurrentETag) && !OldEtag.Equals(CurrentETag))
            //        CreateOrResetEtagCache(CurrentETag, ETagCacheKey);
            //    return response;
            //}

            #endregion 对Get请求做缓存处理

            //else
            //{
            return httpMethod.Method.ToUpper() switch
            {
                "GET" => await httpClient.GetAsync(url),
                "DELETE" => await httpClient.DeleteAsync(url),
                "POST" => await httpClient.PostAsync(url, httpContent),
                "PUT" => await httpClient.PutAsync(url, httpContent),
                "PATCH" => await httpClient.PatchAsync(url, httpContent),
                "HEAD" => await httpClient.HeadAsync(url, httpContent),
                "TRACE" => await httpClient.Tracesync(url, httpContent),
                "OPTIONS" => await httpClient.OptionsAsync(url, httpContent),
                _ => throw new NotSupportedException(nameof(httpMethod)),
            };
            //}
        }

        /// <summary>
        /// 缓存etag
        /// </summary>
        /// <param name="ETag"></param>
        /// <param name="ETagCacheKey"></param>
        private static void CreateOrResetEtagCache(string ETag, string ETagCacheKey)
        {
            if (EtagCaches.ContainsKey(ETagCacheKey))
                EtagCaches.TryRemove(ETagCacheKey,out _);
            EtagCaches.TryAdd(ETagCacheKey, ETag);
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
            return await httpClient.OtherHttpMethodRequest(url, httpContent, HttpMethod.Options);
        }

        private static async Task<HttpResponseMessage> Tracesync(this HttpClient httpClient, string url, HttpContent httpContent)
        {
            return await httpClient.OtherHttpMethodRequest(url, httpContent, HttpMethod.Trace);
        }

        private static async Task<HttpResponseMessage> OtherHttpMethodRequest(this HttpClient httpClient, string url, HttpContent httpContent, HttpMethod httpMethod)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage
            {
                Content = httpContent,
                RequestUri = new Uri(url),
                Method = httpMethod
            };
            return await httpClient.SendAsync(httpRequestMessage);
        }

        /// <summary>
        /// 构建HttpContent
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static HttpContent BuildHttpContent<T>(this T t) where T : class
        {
            if (t is null)
                return new StringContent("");
            var requestJson = JsonConvert.SerializeObject(t);
            HttpContent httpContent = new StringContent(requestJson);
            return httpContent;
        }

        /// <summary>
        /// 构建QueryParam
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string BuildHttpQueryParam<T>(this T t, bool MustRefresh = false) where T : class, new()
        {
            if (t is null)
                return MustRefresh ? $"MustRefresh={DateTime.Now.Ticks}" : string.Empty;
            var queryparam = string.Empty;
            var queryKey = t.GetType().FullName;
            if (!QuaryParams.TryGetValue(queryKey, out PropertyInfo[] props))
            {
                props = t.GetType().GetProperties();
                QuaryParams.TryAdd(queryKey, props);
            }
            props = props.Where(p => p.GetValue(t) != null).ToArray();
            if (props.Any())
                queryparam = "?";
            foreach (PropertyInfo prop in props)
            {
                var value = IsNullableEnum(prop.PropertyType) ? (int)prop.GetValue(t) : prop.GetValue(t);
                queryparam += $"{prop.Name}={value}&";
            }
            queryparam = queryparam.TrimEnd('&');
            return MustRefresh ? $"{queryparam}&MustRefresh={DateTime.Now.Ticks}" : queryparam;
        }

        private static bool IsNullableEnum(Type t)
        {
            Type u = Nullable.GetUnderlyingType(t);
            return (u != null) && u.IsEnum;
        }
    }
}