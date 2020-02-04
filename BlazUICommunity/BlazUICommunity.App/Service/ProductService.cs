using Blazui.Community.DTO;
using Blazui.Community.Utility.Extensions;
using Blazui.Community.Utility.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Blazui.Community.App.Service
{
    public class ProductService
    {
        private readonly HttpClient httpClient;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            this.httpClient = httpClientFactory.CreateClient("product");
        }

        public Task<BaseResonse> AddTopic(BZTopicDto  bZTopicDto)
        {
            HttpContent httpContent = CreateContent(bZTopicDto);

            return httpClient.GetJsonAsync<BaseResonse>("api/Topic/Add" , httpContent);
        }

        private static HttpContent CreateContent(object bZTopicDto)
        {
            var requestJson = JsonConvert.SerializeObject(bZTopicDto);
            HttpContent httpContent = new StringContent(requestJson);
            return httpContent;
        }

        public Task<BaseResonse> QueryByUserName(string Account)
        {
            return httpClient.GetJsonAsync<BaseResonse>($"api/user/QueryUserByName/{Account}");
        }
        public Task<BaseResonse> UpdateUser(BZUserUIDto bZUserUIDto)
        {
            HttpContent httpContent = CreateContent(bZUserUIDto);
            return httpClient.GetJsonAsync<BaseResonse>($"api/user/Update/{bZUserUIDto.Id}", httpContent);
        }
    }
}
