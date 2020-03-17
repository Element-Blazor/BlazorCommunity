using Blazui.Community.App.Model.Condition;
using Blazui.Community.DTO;
using Blazui.Community.Enums;
using Blazui.Community.HttpClientExtensions;
using Blazui.Community.Response;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Blazui.Community.App.Service
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
            return await HttpPostJson("api/client/Topic/Add", httpContent);
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="CodeType"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public async Task<BaseResponse> SendVerifyCode(string UserId, VerifyCodeType CodeType, string target)
        {
            return await httpClient.GetJsonResultAsync($"api/client/code/SendVerifyCode/{(int)CodeType}/{UserId}/{target}");
        }

        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="CodeType"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        public async Task<BaseResponse> VerifyVerifyCode(string UserId, VerifyCodeType CodeType, string Code)
        {
            return await httpClient.GetJsonResultAsync($"api/client/code/VerifyCode/{UserId}/{(int)CodeType}/{Code}");
        }

        /// <summary>
        /// 获取我发表的帖子
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<BaseResponse<PageDatas<BZTopicDto>>> GetTopics(SearchPersonalTopicCondition condition)
        {
            HttpContent httpContent = BuildHttpContent(condition);
            return await httpClient.PostWithJsonResultAsync<PageDatas<BZTopicDto>>("api/client/Topic/AppQuery", httpContent);
        }

        /// <summary>
        /// 删除帖子
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<BaseResponse> DelTopic(string Id)
        {
            return await HttpDeleteJson($"api/client/Topic/Delete/{Id}");
        }

        /// <summary>
        /// 获取我收藏的帖子
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<BaseResponse<PageDatas<BZTopicDto>>> GetFollows(SearchPersonalFollowCondition condition)
        {
            HttpContent httpContent = BuildHttpContent(condition);
            return await httpClient.GetJsonResultAsync<PageDatas<BZTopicDto>>("api/client/follow/Query", HttpClientExtensions.HttpMethod.Post, httpContent);
        }

        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<BaseResponse> DelFollows(string TopicId, string UserId)
        {
            return await HttpDeleteJson($"api/client/follow/Cancel/{TopicId}/{UserId}");
        }

        /// <summary>
        /// 获取我的回帖
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<BaseResponse<List<BZReplyDto>>> GetMyReplys(string UserId, int pageIndex, int pageSize, string Title)
        {
            if (string.IsNullOrWhiteSpace(Title))
                return await httpClient.GetJsonResultAsync<List<BZReplyDto>>($"api/client/reply/GetByUserId/{UserId}/{pageSize}/{pageIndex}");
            return await httpClient.GetJsonResultAsync<List<BZReplyDto>>($"api/client/reply/GetByUserId/{UserId}/{pageSize}/{pageIndex}/{Title}");
        }

        /// <summary>
        /// 获取我的回帖
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<BaseResponse<int>> GetMyReplysCount(string UserId, string Title)
        {
            if (string.IsNullOrWhiteSpace(Title))
                return await httpClient.GetJsonResultAsync<int>($"api/client/reply/GetRepyCount/{UserId}");
            return await httpClient.GetJsonResultAsync<int>($"api/client/reply/GetRepyCount/{UserId}/{Title}");
        }

        /// <summary>
        /// 删除回帖
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<BaseResponse> DelRelpy(string ReplyId)
        {
            return await HttpDeleteJson($"api/client/reply/Delete/{ReplyId}");
        }

        /// <summary>
        /// 获取置顶的帖子（前3条-时间倒序）
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<List<BZTopicDto>>> GetTopdTopics()
        {
            return await httpClient.GetJsonResultAsync<List<BZTopicDto>>($"api/client/topic/top/{3}");
        }

        /// <summary>
        /// 根据排序类型分页查询
        /// </summary>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<BaseResponse<PageDatas<BZTopicDto>>> GetTopicsByOrder(int orderBy, int topicType, int pageIndex, int pageSize)
        {
            return await httpClient.GetJsonResultAsync<PageDatas<BZTopicDto>>($"api/client/topic/QueryTopicsByOrder/{orderBy}/{topicType}/{pageSize}/{pageIndex}");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="title"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<BaseResponse<PageDatas<SeachTopicDto>>> SeachTopicByTitle(string title, int pageIndex, int pageSize)
        {
            return await httpClient.GetJsonResultAsync<PageDatas<SeachTopicDto>>($"api/client/topic/SeachTopicByTitle/{title}/{pageIndex}/{pageSize}");
        }

        /// <summary>
        /// 根据帖子Id查询帖子
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public async Task<BaseResponse<BZTopicDto>> GetTopicById(string topicId)
        {
            return await httpClient.GetJsonResultAsync<BZTopicDto>($"api/client/topic/Query/{topicId}");
        }

        /// <summary>
        /// 修改主贴内容
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<BaseResponse> UpdateTopic(BZTopicDto dto)
        {
            HttpContent httpContent = BuildHttpContent(dto);
            return await HttpPostJson($"api/client/topic/UpdateContent", httpContent);
        }

        /// <summary>
        /// 修改回贴内容
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<BaseResponse> UpdateReply(BZReplyDto dto)
        {
            HttpContent httpContent = BuildHttpContent(dto);
            return await HttpPostJson($"api/client/reply/UpdateContent", httpContent);
        }

        /// <summary>
        /// 根据帖子Id查询回帖
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public async Task<BaseResponse<PageDatas<BZReplyDto>>> GetReplys(string topicId, int pageindex, int pagesize)
        {
            return await httpClient.GetJsonResultAsync<PageDatas<BZReplyDto>>($"api/client/topic/Reply/{topicId}/{pagesize}/{pageindex}");
        }

        /// <summary>
        /// 回帖
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public async Task<BaseResponse> AddReply(BZReplyDto bZReplyDto)
        {
            HttpContent httpContent = BuildHttpContent(bZReplyDto);
            return await HttpPostJson($"api/client/Reply/Add", httpContent);
        }

        /// <summary>
        /// 获取版本数据
        /// </summary>
        /// <returns></returns>
        public async Task<BaseResponse<List<BZVersionDto>>> GetVersions(int Project)
        {
            return await httpClient.GetJsonResultAsync<List<BZVersionDto>>($"api/client/Version/Query/{Project}");
        }

        /// <summary>
        /// 获取版本数据
        /// </summary>
        /// <returns></returns>
        public async Task<BaseResponse<List<BZVersionDto>>> GetAllVersions()
        {
            return await httpClient.GetJsonResultAsync<List<BZVersionDto>>($"api/client/Version/GetAll");
        }

        /// <summary>
        /// 检查是否收藏了该帖子
        /// </summary>
        /// <returns></returns>
        public async Task<BaseResponse<BZFollowDto>> IsStar(string UserId, string TopicId)
        {
            return await httpClient.GetJsonResultAsync<BZFollowDto>($"api/client/Follow/IsStar/{UserId}/{TopicId}");
        }

        /// <summary>
        ///  改变是否收藏状态
        /// </summary>
        /// <returns></returns>
        public async Task<BaseResponse> ToggleFollow(BZFollowDto dto)
        {
            HttpContent httpContent = BuildHttpContent(dto);
            return await HttpPostJson($"api/client/Follow/Toggle", httpContent);
        }

        public async Task<BaseResponse<List<BzBannerDto>>> GetBanners()
        {
            return await httpClient.GetJsonResultAsync<List<BzBannerDto>>($"api/client/Banner/QueryAll");
        }

        #region 在包一层

        private async Task<BaseResponse> HttpGetJson(string url)
        {
            var ResultResponse = await _tokenService.RquestToken();
            if (!ResultResponse.IsSuccess)
                return Unauthorized;
            SetHttpClientAuthorization(ResultResponse);
            return await httpClient.GetJsonResultAsync(url);
        }

        private async Task<BaseResponse<T>> HttpPostJson<T>(string url, HttpContent httpContent = null)
        {
            var ResultResponse = await _tokenService.RquestToken();
            if (!ResultResponse.IsSuccess)
                return new BaseResponse<T>(401);
            SetHttpClientAuthorization(ResultResponse);
            return await httpClient.PostWithJsonResultAsync<T>(url, httpContent);
        }

        private async Task<BaseResponse> HttpPostJson(string url, HttpContent httpContent = null)
        {
            var ResultResponse = await _tokenService.RquestToken();
            if (!ResultResponse.IsSuccess)
                return Unauthorized;
            SetHttpClientAuthorization(ResultResponse);
            return await httpClient.PostWithJsonResultAsync(url, httpContent);
        }

        private async Task<BaseResponse> HttpDeleteJson(string url)
        {
            var ResultResponse = await _tokenService.RquestToken();
            if (!ResultResponse.IsSuccess)
                return Unauthorized;
            SetHttpClientAuthorization(ResultResponse);
            return await httpClient.DeleteWithJsonResultAsync(url);
        }

        private void SetHttpClientAuthorization(BaseResponse<Token> ResultResponse)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ResultResponse.Data.AccessToken);
        }

        #endregion 在包一层
    }
}