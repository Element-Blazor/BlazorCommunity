using Blazui.Community.Utility.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Blazui.Community.Utility.Extensions
{
  public static class HttpClientExtension
    {
        private static async Task<BaseResponse<T>> GetResult<T>(this HttpClient httpClient , string url,HttpMethod httpMethod=HttpMethod.Get)
        {
            if ( httpClient is null )
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            if ( string.IsNullOrEmpty(url) )
            {
                throw new ArgumentException("url is null" , nameof(url));
            }
            HttpResponseMessage response = null;
            switch (httpMethod)
            {
                case HttpMethod.Get:
                    response = await httpClient.GetAsync(url);
                    break;
                case HttpMethod.Post:
                    break;
                case HttpMethod.Delete:
                    response = await httpClient.DeleteAsync(url);
                    break;
                case HttpMethod.Put:
                    //response = await httpClient.PutAsync(url);
                    break;
                default:
                    break;
            }
            var header = response.Headers.GetValues("ETag");
            if (header != null && header.Any())
            {
                httpClient.DefaultRequestHeaders.Remove("If-None-Match");
                httpClient.DefaultRequestHeaders.Add("If-None-Match", header.First());
            }
            if (response.StatusCode == System.Net.HttpStatusCode.NotModified)
                return new BaseResponse<T>((int)response.StatusCode);
        
            return await DeserializeHttpResponseMessage<BaseResponse<T>>(response);
        }
        private static async Task<T> GetResult1<T>(this HttpClient httpClient, string url, HttpMethod httpMethod = HttpMethod.Get)
        {
            if (httpClient is null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("url is null", nameof(url));
            }
            HttpResponseMessage response = null;
            switch (httpMethod)
            {
                case HttpMethod.Get:
                    response = await httpClient.GetAsync(url);
                    break;
                case HttpMethod.Post:
                    break;
                case HttpMethod.Delete:
                    response = await httpClient.DeleteAsync(url);
                    break;
                case HttpMethod.Put:
                    //response = await httpClient.PutAsync(url);
                    break;
                default:
                    break;
            }
            //if (response.StatusCode == System.Net.HttpStatusCode.NotModified)

            var header = response.Headers.GetValues("ETag");
            if (header != null && header.Any())
                httpClient.DefaultRequestHeaders.Add("If-None-Match", header.First());
            return await DeserializeHttpResponseMessage<T>(response);
        }

        private static async Task<T> PostResult<T>(this HttpClient httpClient , string url, HttpContent httpContent)
        {
            if (httpClient is null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("url is null", nameof(url));
            }

            if (httpContent is null)
            {
                throw new ArgumentNullException(nameof(httpContent));
            }

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PostAsync(url, httpContent);
            var header = response.Headers.GetValues("ETag");
            if (header != null && header.Any())
                httpClient.DefaultRequestHeaders.Add("If-None-Match", header.First());
            return await DeserializeHttpResponseMessage<T>(response);
        }

        private static async Task<T> DeserializeHttpResponseMessage<T>(HttpResponseMessage response)
        {
            if (response is null ||response.StatusCode== System.Net.HttpStatusCode.NotModified)
                return default;
            //response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);
            try
            {
                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (Exception ex)
            {
                return default;
            }
        }



        private static BaseResponse Result(BaseResponse response)
        {
            if (response == null)
                return new BaseResponse();
            return response;
        }
        private static BaseResponse<T> Result<T>(BaseResponse<T> response)
        {
            if (response == null)
                return new BaseResponse<T>();
            return response;
        }
        public static async Task<BaseResponse<T>> PostJsonAsync<T>(this HttpClient httpClient,string url, HttpContent httpContent = null)
        {
            return Result(await httpClient.PostResult<BaseResponse<T>>(url, httpContent));
        }
        public static async Task<BaseResponse> PostJsonAsync(this HttpClient httpClient,string url, HttpContent httpContent = null)
        {
            return Result(await httpClient.PostResult<BaseResponse>(url, httpContent));
        }

        public static async Task<BaseResponse<T>> GetJsonAsync<T>(this HttpClient httpClient,string url, HttpMethod method = HttpMethod.Get)
        {
            return Result(await httpClient.GetResult<T>(url, method));
        }
        public static async Task<BaseResponse> GetJsonAsync(this HttpClient httpClient,string url, HttpMethod method = HttpMethod.Get)
        {
            return Result(await httpClient.GetResult1<BaseResponse>(url, method));
        }
    }


    public enum HttpMethod
    {
        Get,
        Post,
        Delete,
         Put,
    }
}
