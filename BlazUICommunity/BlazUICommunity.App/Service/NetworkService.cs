using Blazui.Community.App.Model;
using Blazui.Community.App.Model.Condition;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using Blazui.Community.Utility.Extensions;
using Blazui.Community.Utility.Request;
using Blazui.Community.Utility.Response;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HttpMethod = Blazui.Community.Utility.Extensions.HttpMethod;

namespace Blazui.Community.App.Service
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
        /// <param name="bZTopicDto"></param>
        /// <returns></returns>
        private static HttpContent CreateContent(object bZTopicDto)
        {
            var requestJson = JsonConvert.SerializeObject(bZTopicDto);
            HttpContent httpContent = new StringContent(requestJson);
            return httpContent;
        }
        /// <summary>
        /// 发表帖子
        /// </summary>
        /// <param name="bZTopicDto"></param>
        /// <returns></returns>
        public Task<BaseResponse> AddTopic(BZTopicDto bZTopicDto)
        {
            HttpContent httpContent = CreateContent(bZTopicDto);

            return httpClient.GetJsonAsync<BaseResponse>("api/Topic/Add", httpContent);
        }



        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="CodeType"></param>
        /// <param name="Mobile"></param>
        /// <returns></returns>
        public Task<BaseResponse> CreateAndSendVerifyCodeMessage(int UserId, int CodeType, string Mobile)
        {
            return httpClient.GetJsonAsync<BaseResponse>($"api/code/CreateCode/{UserId}/{CodeType}/{Mobile}");
        }
        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="CodeType"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        public Task<BaseResponse> VerifyVerifyCode(int UserId, int CodeType, string Code)
        {
            return httpClient.GetJsonAsync<BaseResponse>($"api/code/VerifyCode/{UserId}/{CodeType}/{Code}");
        }

        /// <summary>
        /// 获取我发表的帖子
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public Task<BaseResponse<PageDatas<BZTopicDto>>> GetTopics(SearchPersonalTopicCondition condition)
        {
            HttpContent httpContent = CreateContent(condition);
            return httpClient.GetJsonAsync<BaseResponse<PageDatas<BZTopicDto>>>($"api/Topic/Query", httpContent);
        }
        /// <summary>
        /// 删除帖子
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Task<BaseResponse> DelTopic(int Id)
        {
            return httpClient.GetJsonAsync<BaseResponse>($"api/Topic/Delete/{Id}", HttpMethod.Delete);
        }

        /// <summary>
        /// 获取我收藏的帖子
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public Task<BaseResponse<PageDatas<BZTopicDto>>> GetFollows(SearchPersonalFollowCondition condition)
        {
            HttpContent httpContent = CreateContent(condition);
            return httpClient.GetJsonAsync<BaseResponse<PageDatas<BZTopicDto>>>($"api/follow/Query", httpContent);
        }

        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Task<BaseResponse> DelFollows(int Id)
        {
            return httpClient.GetJsonAsync<BaseResponse>($"api/follow/delete/{Id}", HttpMethod.Delete);
        }


        /// <summary>
        /// 获取置顶的帖子（前3条-时间倒序）
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Task<BaseResponse<List<BZTopicDtoWithUser>>> GetTopdTopics()
        {
            return httpClient.GetJsonAsync<BaseResponse<List<BZTopicDtoWithUser>>>($"api/topic/top/{3}", HttpMethod.Get);
        }

        /// <summary>
        /// 根据排序类型分页查询
        /// </summary>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Task<BaseResponse<PageDatas<BZTopicDtoWithUser>>> GetTopicsByOrder(int orderBy, int topicType, int pageIndex, int pageSize)
        {
            return httpClient.GetJsonAsync<BaseResponse<PageDatas<BZTopicDtoWithUser>>>($"api/topic/QueryTopicsByOrder/{orderBy}/{topicType}/{pageSize}/{pageIndex}", HttpMethod.Get);
        }


        /// <summary>
        /// 根据帖子Id查询帖子
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public Task<BaseResponse<BZTopicDtoWithUser>> GetTopicById(int topicId)
        {
            return httpClient.GetJsonAsync<BaseResponse<BZTopicDtoWithUser>>($"api/topic/Query/{topicId}", HttpMethod.Get);
        }
        /// <summary>
        /// 根据帖子Id查询帖子
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public Task<BaseResponse<PageDatas<BZReplyDtoWithUser>>> GetReplys(int topicId,int pageindex,int pagesize)
        {
            return httpClient.GetJsonAsync<BaseResponse<PageDatas<BZReplyDtoWithUser>>>($"api/topic/Reply/{topicId}/{pagesize}/{pageindex}", HttpMethod.Get);
        }
        /// <summary>
        /// 回帖
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public Task<BaseResponse> AddReply(BZReplyDto bZReplyDto)
        {
            HttpContent httpContent = CreateContent(bZReplyDto);
            return httpClient.GetJsonAsync<BaseResponse>($"api/Reply/Add", httpContent);
        }
    }
}
