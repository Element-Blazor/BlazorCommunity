using BlazorCommunity.App.Model.Condition;
using BlazorCommunity.DTO;
using BlazorCommunity.Enums;
using BlazorCommunity.HttpClientExtensions;
using BlazorCommunity.Response;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BlazorCommunity.App.Service
{
    public class NetworkService
    {
        private readonly HttpClient httpClient;
        private readonly TokenService _tokenService;
        private readonly BaseResponse Unauthorized;

      

        public NetworkService(IHttpClientFactory httpClientFactory, TokenService tokenService)
        {
            this.httpClient = httpClientFactory.CreateClient("BlazuiCommunitiyApp");
            _tokenService = tokenService;
            Unauthorized = new BaseResponse(403, "Unauthorized ，对不起您没有权限进行该操作 ", null);
        }

        




        #region User
        internal Task<BaseResponse<List<HotUserDto>>> QueryHotUsers()
        {
            return  httpClient.GetWithJsonResultAsync<List<HotUserDto>>($"api/client/user/Hot");
        }

        internal async Task<HotUserDto> QueryTopicUser(string topicId)
        {
            var result= await httpClient.GetWithJsonResultAsync<HotUserDto>($"api/client/user/TopicUser/{topicId}");
            if (result.IsSuccess)
                return result.Data;
            else return new HotUserDto();
        }
        #endregion

        #region VerifyCode

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="CodeType"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public async Task<BaseResponse> SendVerifyCode(string UserId, EmailType CodeType, string target)
        {
            return await httpClient.GetWithJsonResultAsync($"api/client/code/SendVerifyCode/{(int)CodeType}/{UserId}/{target}");
        }

        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="CodeType"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        public async Task<BaseResponse> ValidateVerifyCode(string UserId, EmailType CodeType, string Code)
        {
            return await httpClient.GetWithJsonResultAsync($"api/client/code/VerifyCode/{UserId}/{(int)CodeType}/{Code}");
        }

        #endregion VerifyCode

        #region Topic

        /// <summary>
        /// 发表帖子
        /// </summary>
        /// <param name="bZTopicDto"></param>
        /// <returns></returns>
        public async Task<BaseResponse> AddTopic(BZTopicDto bZTopicDto,bool Notice)
        {
            HttpContent httpContent = bZTopicDto.BuildHttpContent();
            var notice = Notice ? 1 : 0;
            return await HttpRequestWithValidate($"api/client/Topic/Add/{notice}", HttpMethod.Post, httpContent);
        }

        /// <summary>
        /// 获取我发表的帖子
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<BaseResponse<PageDatas<PersonalTopicDisplayDto>>> QueryPersonalTopics(SearchPersonalTopicCondition condition, bool MustRefresh = false)
        {
            return await httpClient.GetWithJsonResultAsync<PageDatas<PersonalTopicDisplayDto>>($"api/client/Topic/Query{condition.BuildHttpQueryParam(MustRefresh)}");
        }

        /// <summary>
        /// 删除帖子
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<BaseResponse> DeleteTopic(string Id)
        {
            return await HttpRequestWithValidate($"api/client/Topic/Delete/{Id}", HttpMethod.Delete);
        }

        /// <summary>
        /// 获取置顶的帖子（前3条-时间倒序）
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<List<BZTopicDto>>> QueryTopdTopics()
        {
            return await httpClient.GetWithJsonResultAsync<List<BZTopicDto>>($"api/client/topic/top/{3}");
        }

        /// <summary>
        /// 根据排序类型分页查询
        /// </summary>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<BaseResponse<PageDatas<BZTopicDto>>> QueryTopicsByOrder(int orderBy, int topicType, int pageIndex, int pageSize)
        {
            return await httpClient.GetWithJsonResultAsync<PageDatas<BZTopicDto>>($"api/client/topic/QueryByOrder/{orderBy}/{topicType}/{pageSize}/{pageIndex}");
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<BaseResponse<PageDatas<BZTopicDto>>> QueryTopicsWithPage(int pageIndex, int pageSize)
        {
            return await httpClient.GetWithJsonResultAsync<PageDatas<BZTopicDto>>($"api/client/topic/QueryByPage/{pageSize}/{pageIndex}");
        }

        /// <summary>
        /// 移动端分页查询
        /// </summary>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<BaseResponse<PageDatas<BZTopicDto>>> MobileQuery(int pageIndex, int pageSize)
        {
            return await httpClient.GetWithJsonResultAsync<PageDatas<BZTopicDto>>($"api/client/topic/MobileQuery/{pageSize}/{pageIndex}");
        }
        
        /// <summary>
        ///首页搜索
        /// </summary>
        /// <param name="title"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<BaseResponse<PageDatas<SeachTopicDto>>> SeachTopicByTitle(string title, int pageIndex, int pageSize)
        {
            return await httpClient.GetWithJsonResultAsync<PageDatas<SeachTopicDto>>($"api/client/topic/SeachByTitle/{title}/{pageIndex}/{pageSize}");
        }

        /// <summary>
        /// 根据帖子Id查询帖子
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public async Task<BaseResponse<BZTopicDto>> QueryTopicById(string topicId)
        {
            return await httpClient.GetWithJsonResultAsync<BZTopicDto>($"api/client/topic/Query/{topicId}");
        }

        /// <summary>
        /// 修改主贴内容
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<BaseResponse> UpdateTopic(BZTopicDto dto)
        {
            return await HttpRequestWithValidate($"api/client/topic/UpdateContent", HttpMethod.Patch, dto.BuildHttpContent());
        }

        /// <summary>
        /// 发帖人关闭帖子
        /// </summary>
        /// <param name="TopicId"></param>
        /// <returns></returns>
        public async Task<BaseResponse> EndTopic(string TopicId)
        {
            return await HttpRequestWithValidate($"api/client/topic/End/{TopicId}", HttpMethod.Patch);
        }


        internal async Task<BaseResponse<List<HotTopicDto>>> QueryShareHot()
        {
            return await httpClient.GetWithJsonResultAsync<List<HotTopicDto>>($"api/client/topic/ShareHot");
        }
        internal async Task<BaseResponse<List<HotTopicDto>>> QueryAskHot()
        {
            return await httpClient.GetWithJsonResultAsync<List<HotTopicDto>>($"api/client/topic/AskHot");
        }

        internal async Task<BaseResponse<List<HotTopicDto>>> QueryTopicByAuthor(string topicId)
        {
            return await httpClient.GetWithJsonResultAsync<List<HotTopicDto>>($"api/client/topic/QueryTopicByAuthor/{topicId}");
        }
        #endregion Topic

        #region Reply

        /// <summary>
        /// 删除回帖
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<BaseResponse> DeleteRelpy(string ReplyId)
        {
            return await HttpRequestWithValidate($"api/client/reply/Delete/{ReplyId}", HttpMethod.Delete);
        }

        /// <summary>
        /// 获取我的回帖
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<BaseResponse<PageDatas<PersonalReplyDisplayDto>>> QueryPersonalReplys(string UserId, int pageIndex, int pageSize, string Title)
        {
            var url = $"api/client/reply/GetByUserId/{UserId}/{pageSize}/{pageIndex}";
            return await httpClient.GetWithJsonResultAsync<PageDatas<PersonalReplyDisplayDto>>(
                string.IsNullOrWhiteSpace(Title) ? url : url + $"/{Title}");
        }

        /// <summary>
        /// 修改回贴内容
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<BaseResponse> UpdateReply(BZReplyDto dto)
        {
            return await HttpRequestWithValidate($"api/client/reply/UpdateContent", HttpMethod.Patch, dto.BuildHttpContent());
        }

        /// <summary>
        /// 新增回帖
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public async Task<BaseResponse> AddReply(BZReplyDto bZReplyDto)
        {
            HttpContent httpContent = bZReplyDto.BuildHttpContent();
            return await HttpRequestWithValidate($"api/client/Reply/Add", HttpMethod.Post, httpContent);
        }

        /// <summary>
        /// 根据帖子Id查询回帖
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public async Task<BaseResponse<PageDatas<BZReplyDto>>> QueryReplysByTopicId(string topicId, int pageindex, int pagesize)
        {
            return await httpClient.GetWithJsonResultAsync<PageDatas<BZReplyDto>>($"api/client/topic/Reply/{topicId}/{pagesize}/{pageindex}");
        }

        #endregion Reply

        #region Version

        /// <summary>
        /// 获取版本数据
        /// </summary>
        /// <returns></returns>
        public async Task<BaseResponse<List<BZVersionDto>>> QueryAllVersions()
        {
            return await httpClient.GetWithJsonResultAsync<List<BZVersionDto>>($"api/client/Version/GetAll");
        }

        /// <summary>
        /// 获取版本数据
        /// </summary>
        /// <returns></returns>
        public async Task<BaseResponse<List<BZVersionDto>>> QueryVersionsByProjectId(int Project)
        {
            return await httpClient.GetWithJsonResultAsync<List<BZVersionDto>>($"api/client/Version/Query/{Project}");
        }

        #endregion Version

        #region Banner

        public async Task<BaseResponse<List<BzBannerDto>>> QueryBanners()
        {
            return await httpClient.GetWithJsonResultAsync<List<BzBannerDto>>($"api/client/Banner/QueryAll");
        }

        #endregion Banner

        #region Follow

        /// <summary>
        ///  切换是否收藏
        /// </summary>
        /// <returns></returns>
        public async Task<BaseResponse> ToggleFollow(BZFollowDto dto)
        {
            return await HttpRequestWithValidate($"api/client/Follow/Toggle", HttpMethod.Patch, dto.BuildHttpContent());
        }

        /// <summary>
        /// 查询是否已收藏帖子
        /// </summary>
        /// <returns></returns>
        public async Task<BaseResponse<BZFollowDto>> IsFollowed(string UserId, string TopicId)
        {
            return await httpClient.GetWithJsonResultAsync<BZFollowDto>($"api/client/Follow/IsFollowed/{UserId}/{TopicId}");
        }

        /// <summary>
        /// 获取我收藏的帖子
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<BaseResponse<PageDatas<PersonalFollowDisplayDto>>> QueryFollows(SearchPersonalFollowCondition condition)
        {
            return await httpClient.GetWithJsonResultAsync<PageDatas<PersonalFollowDisplayDto>>($"api/client/follow/Query{condition.BuildHttpQueryParam()}");
        }

        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<BaseResponse> CancelFollow(string FollowId)
        {
            return await HttpRequestWithValidate($"api/client/follow/Cancel/{FollowId}", HttpMethod.Patch);
        }

        #endregion Follow

        #region 处理需要授权才能访问的接口

        private async Task<BaseResponse> HttpRequestWithValidate(string url, HttpMethod httpMethod, HttpContent httpContent = null)
        {
            var ResultResponse = await _tokenService.RquestToken();
            if (!ResultResponse.IsSuccess)
                return Unauthorized;
            AppdendAuthorizationToken(ResultResponse);
            return httpMethod.Method.ToUpper() switch
            {
                "GET" => await httpClient.GetWithJsonResultAsync(url),
                "DELETE" => await httpClient.DeleteWithJsonResultAsync(url),
                "POST" => await httpClient.PostWithJsonResultAsync(url, httpContent),
                "PUT" => await httpClient.PutWithJsonResultAsync(url, httpContent),
                "PATCH" => await httpClient.PatchWithJsonResultAsync(url, httpContent),
                _ => throw new NotSupportedException(nameof(httpMethod)),
            };
        }

        private void AppdendAuthorizationToken(BaseResponse<Token> ResultResponse)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ResultResponse.Data.AccessToken);
        }

        #endregion 处理需要授权才能访问的接口
    }
}