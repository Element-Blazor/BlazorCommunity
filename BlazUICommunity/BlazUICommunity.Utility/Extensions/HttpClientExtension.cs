using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Blazui.Community.Utility.Extensions
{
  public static class HttpClientExtension
    {
        public static async Task<T> GetJsonAsync<T>(this HttpClient httpClient , string url,HttpMethod httpMethod=HttpMethod.Get)
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
                    break;
                default:
                    break;
            }
            return await DeserializeHttpResponseMessage<T>(response);
        }

     
        public static async Task<T> GetJsonAsync<T>(this HttpClient httpClient , string url, HttpContent httpContent)
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
            return await DeserializeHttpResponseMessage<T>(response);
        }

        private static async Task<T> DeserializeHttpResponseMessage<T>(HttpResponseMessage response)
        {
            if (response is null)
                return default;
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
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
