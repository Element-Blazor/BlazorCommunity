using Blazui.Community.Admin.QueryCondition;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using Blazui.Community.Utility.Extensions;
using Blazui.Community.Utility.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HttpMethod = Blazui.Community.Utility.Extensions.HttpMethod;

namespace Blazui.Community.Admin.Service
{
    public class NetworkService
    {
        private readonly HttpClient httpClient;

        public NetworkService(IHttpClientFactory httpClientFactory)
        {
            this.httpClient = httpClientFactory.CreateClient("product");
        }
        /// <summary>
        /// 构建HttpContent
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        private static HttpContent BuildHttpContent<T>(T t)
        {
            if (t is null)
                return new StringContent("");
            var requestJson = JsonConvert.SerializeObject(t);
            HttpContent httpContent = new StringContent(requestJson);
            return httpContent;
        }



        /// <summary>
        /// 查询用户
        /// </summary>
        /// <returns></returns>
        internal async Task <BaseResponse<PageDatas<BZUserUIDto>>> QueryUsers(QueryUserCondition querycondition)
        {
          return  await httpClient.PostJsonAsync<PageDatas<BZUserUIDto>>("api/user/Query", BuildHttpContent(querycondition));
        }
        /// <summary>
        /// 查询用户
        /// </summary>
        /// <returns></returns>
        internal async Task<BaseResponse<BZUserUIDto> >QueryUserByUserName(string UserName)
        {
            return await httpClient.GetJsonAsync<BZUserUIDto>($"api/user/QueryByName/{UserName}");
        }
        /// <summary>
        /// 封禁账号
        /// </summary>
        /// <returns></returns>
        internal async Task<BaseResponse> FrozenUser(int UserId)
        {
            return await httpClient.GetJsonAsync($"api/user/Frozen/{UserId}");
        }
        /// <summary>
        /// 解封
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> UnFrozen(int UserId)
        {
            return await httpClient.GetJsonAsync($"api/user/UnFrozen/{UserId}");
        }

   

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> ResetPassword(int UserId)
        {
            return await httpClient.GetJsonAsync($"api/user/ResetPassword/{UserId}");
        }
        /// <summary>
        /// 查询帖子
        /// </summary>
        /// <param name="querycondition"></param>
        /// <returns></returns>
        internal async Task<BaseResponse<PageDatas<BZTopicDtoWithUser>>> QueryTopics(QueryTopicCondition querycondition,string username="")
        {
            var url = "api/Topic/Query";
            if (!string.IsNullOrWhiteSpace(username))
                url += $"?username={username}";
            return await httpClient.PostJsonAsync<PageDatas<BZTopicDtoWithUser>>(url, BuildHttpContent(querycondition));
        }
        /// <summary>
        /// 查询回复贴
        /// </summary>
        /// <param name="querycondition"></param>
        /// <returns></returns>
        internal async Task<BaseResponse<PageDatas<BZReplyDtoWithUser>>> QueryReplys(QueryReplyCondition querycondition,string username,string title)
        {
            var url = "api/Reply/QueryReplys";
            if (!string.IsNullOrWhiteSpace(username)&& !string.IsNullOrWhiteSpace(title))
            {
                url += $"?username={username}";
                url += $"&topicTitle={title}";
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(username))
                {
                    url += $"?username={username}";
                }
                if (!string.IsNullOrWhiteSpace(title))
                {
                    url += $"?topicTitle={title}";
                }
            }
            return await httpClient.PostJsonAsync<PageDatas<BZReplyDtoWithUser>>(url, BuildHttpContent(querycondition));
        }

     
        /// <summary>
        /// 发表帖子
        /// </summary>
        /// <param name="bZTopicDto"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> NewTopic(BZTopicDto bZTopicDto)
        {
            HttpContent httpContent = BuildHttpContent(bZTopicDto);

            return await httpClient.PostJsonAsync("api/Topic/Add", httpContent);
        }



        /// <summary>
        /// 删除帖子
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> DelTopic(int Id,int status)
        {
            if(status==-1)
                return await httpClient.GetJsonAsync($"api/Topic/Active/{Id}", HttpMethod.Delete);
            return await httpClient.GetJsonAsync($"api/Topic/Delete/{Id}", HttpMethod.Delete);
        }

        /// <summary>
        /// 加精
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> BestTopic(int Id)
        {
            return await httpClient.GetJsonAsync($"api/Topic/BestTopic/{Id}");
        }
        /// <summary>
        /// 置顶
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> TopTopic(int Id)
        {
            return await httpClient.GetJsonAsync($"api/Topic/TopTopic/{Id}");
        }

        /// <summary>
        /// 结贴
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> EndTopic(int Id)
        {
            return await httpClient.GetJsonAsync($"api/Topic/EndTopic/{Id}");
        }

        /// <summary>
        /// 删除或恢复回帖
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> DelReply(int Id)
        {
            return await httpClient.GetJsonAsync($"api/Reply/DeleteOrActive/{Id}",HttpMethod.Delete);
        }
    }
}
