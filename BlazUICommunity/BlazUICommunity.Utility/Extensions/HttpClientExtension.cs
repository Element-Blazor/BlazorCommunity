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
        public static async Task<T> GetJsonAsync<T>(this HttpClient httpClient , string url)
        {
            if ( httpClient is null )
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            if ( string.IsNullOrEmpty(url) )
            {
                throw new ArgumentException("url is null" , nameof(url));
            }

            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }


        public static async Task<T> GetJsonAsync<T>(this HttpClient httpClient , string url, HttpContent httpContent)
        {
            if ( httpClient is null )
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            if ( string.IsNullOrEmpty(url) )
            {
                throw new ArgumentException("url is null" , nameof(url));
            }

            if ( httpContent is null )
            {
                throw new ArgumentNullException(nameof(httpContent));
            }

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PostAsync(url , httpContent);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
