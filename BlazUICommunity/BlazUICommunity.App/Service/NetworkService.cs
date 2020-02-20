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
        /// 发表帖子
        /// </summary>
        /// <param name="bZTopicDto"></param>
        /// <returns></returns>
        public async Task<BaseResponse> AddTopic(BZTopicDto bZTopicDto)
        {
            HttpContent httpContent = BuildHttpContent(bZTopicDto);
            return await httpClient.PostJsonAsync("api/Topic/Add", httpContent);
        }



        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="CodeType"></param>
        /// <param name="Mobile"></param>
        /// <returns></returns>
        public async Task<BaseResponse> CreateAndSendVerifyCodeMessage(int UserId, int CodeType, string Mobile)
        {
            return await httpClient.GetJsonAsync($"api/code/CreateCode/{UserId}/{CodeType}/{Mobile}");
        }
        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="CodeType"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        public async Task<BaseResponse> VerifyVerifyCode(int UserId, int CodeType, string Code)
        {
            return await httpClient.GetJsonAsync($"api/code/VerifyCode/{UserId}/{CodeType}/{Code}");
        }

        /// <summary>
        /// 获取我发表的帖子
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<BaseResponse<PageDatas<BZTopicDto>>> GetTopics(SearchPersonalTopicCondition condition)
        {
            HttpContent httpContent = BuildHttpContent(condition);
            return await httpClient.PostJsonAsync<PageDatas<BZTopicDto>>("api/Topic/AppQuery", httpContent);
        }
        /// <summary>
        /// 删除帖子
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<BaseResponse> DelTopic(int Id)
        {
            return await httpClient.GetJsonAsync($"api/Topic/Delete/{Id}", HttpMethod.Delete);
        }

        /// <summary>
        /// 获取我收藏的帖子
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<BaseResponse<PageDatas<BZTopicDto>>> GetFollows(SearchPersonalFollowCondition condition)
        {
            HttpContent httpContent = BuildHttpContent(condition);
            return await httpClient.PostJsonAsync<PageDatas<BZTopicDto>>("api/follow/Query", httpContent);
        }

        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<BaseResponse> DelFollows(int TopicId, int UserId)
        {
            return await httpClient.GetJsonAsync($"api/follow/Cancel/{TopicId}/{UserId}", HttpMethod.Delete);
        }

        /// <summary>
        /// 获取我的回帖
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<BaseResponse<List<ReplyDto>>> GetMyReplys(int UserId, int pageIndex, int pageSize)
        {

            return await httpClient.GetJsonAsync<List<ReplyDto>>($"api/reply/GetByUserId/{UserId}/{pageSize}/{pageIndex}");
        }

        /// <summary>
        /// 删除回帖
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<BaseResponse> DelRelpy(int ReplyId)
        {
            return await httpClient.GetJsonAsync($"api/reply/Delete/{ReplyId}", HttpMethod.Delete);
        }


        /// <summary>
        /// 获取置顶的帖子（前3条-时间倒序）
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<List<BZTopicDtoWithUser>>> GetTopdTopics()
        {
            return await httpClient.GetJsonAsync<List<BZTopicDtoWithUser>>($"api/topic/top/{3}");
        }

        /// <summary>
        /// 根据排序类型分页查询
        /// </summary>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<BaseResponse<PageDatas<BZTopicDtoWithUser>>> GetTopicsByOrder(int orderBy, int topicType, int pageIndex, int pageSize)
        {
            return await httpClient.GetJsonAsync<PageDatas<BZTopicDtoWithUser>>($"api/topic/QueryTopicsByOrder/{orderBy}/{topicType}/{pageSize}/{pageIndex}");
        }




        /// <summary>
        /// 根据帖子Id查询帖子
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public async Task<BaseResponse<BZTopicDtoWithUser>> GetTopicById(int topicId)
        {
            return await httpClient.GetJsonAsync<BZTopicDtoWithUser>($"api/topic/Query/{topicId}");
        }


     /// <summary>
     /// 修改主贴内容
     /// </summary>
     /// <param name="dto"></param>
     /// <returns></returns>
        public async Task<BaseResponse> UpdateTopic(BZTopicDto dto)
        {
            HttpContent httpContent = BuildHttpContent(dto);
            return await httpClient.PostJsonAsync($"api/topic/UpdateContent", httpContent);
        }

        /// <summary>
        /// 修改回贴内容
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<BaseResponse> UpdateReply(BZReplyDto dto)
        {
            HttpContent httpContent = BuildHttpContent(dto);
            return await httpClient.PostJsonAsync($"api/reply/UpdateContent", httpContent);
        }
        /// <summary>
        /// 根据帖子Id查询回帖
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public async Task<BaseResponse<PageDatas<BZReplyDtoWithUser>>> GetReplys(int topicId, int pageindex, int pagesize)
        {
            return await httpClient.GetJsonAsync<PageDatas<BZReplyDtoWithUser>>($"api/topic/Reply/{topicId}/{pagesize}/{pageindex}");
        }
        /// <summary>
        /// 回帖
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public async Task<BaseResponse> AddReply(BZReplyDto bZReplyDto)
        {
            HttpContent httpContent = BuildHttpContent(bZReplyDto);
            return await httpClient.PostJsonAsync($"api/Reply/Add", httpContent);
        }


        /// <summary>
        /// 获取版本数据
        /// </summary>
        /// <returns></returns>
        public async Task<BaseResponse<List<BZVersionModel>>> GetVersions(int Project)
        {
            return await httpClient.GetJsonAsync<List<BZVersionModel>>($"api/Version/Query/{Project}");
        }
        /// <summary>
        /// 获取版本数据
        /// </summary>
        /// <returns></returns>
        public async Task<BaseResponse<List<BZVersionModel>>> GetAllVersions()
        {
            return await httpClient.GetJsonAsync<List<BZVersionModel>>($"api/Version/GetAll");
        }

        /// <summary>
        /// 检查是否收藏了该帖子
        /// </summary>
        /// <returns></returns>
        public async Task<BaseResponse<BZFollowModel>> IsStar(int UserId, int TopicId)
        {
            return await httpClient.GetJsonAsync<BZFollowModel>($"api/Follow/IsStar/{UserId}/{TopicId}");
        }


        /// <summary>
        ///  改变是否收藏状态
        /// </summary>
        /// <returns></returns>
        public async Task<BaseResponse> ToggleFollow(BZFollowDto dto)
        {
            HttpContent httpContent = BuildHttpContent(dto);
            return await httpClient.PostJsonAsync($"api/Follow/Toggle", httpContent);
        }
    }
}
