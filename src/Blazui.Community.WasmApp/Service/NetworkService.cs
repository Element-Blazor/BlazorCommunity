using Blazored.LocalStorage;
using Blazui.Community.DTO;
using Blazui.Community.DTO.App;
using Blazui.Community.Enums;
using Blazui.Community.HttpClientExtensions;
using Blazui.Community.Response;
using Blazui.Community.Shared;
using Blazui.Community.WasmApp.Model.Cache;
using Blazui.Community.WasmApp.Model.Condition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Blazui.Community.WasmApp.Service
{
    public class NetworkService
    {
        private readonly HttpClient httpClient;



        private readonly ILocalStorageCacheService localStorageService;

        public NetworkService(IHttpClientFactory httpClientFactory, ILocalStorageCacheService localStorageService)
        {
            this.httpClient = httpClientFactory.CreateClient("BlazuiCommunitiyApp");
            this.localStorageService = localStorageService;
        }

        #region User
        internal Task<BaseResponse<List<HotUserDto>>> QueryHotUsers()
        {
            return httpClient.GetWithJsonResultAsync<List<HotUserDto>>($"api/client/user/Hot");
        }

        internal async Task<HotUserDto> QueryTopicUser(string topicId)
        {
            var result = await httpClient.GetWithJsonResultAsync<HotUserDto>($"api/client/user/TopicUser/{topicId}");
            if (result.IsSuccess)
                return result.Data;
            else return new HotUserDto();
        }


        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<BZUserDto> FindUserByIdAsync(string Id)
        {
            var result = await httpClient.GetWithJsonResultAsync<BZUserDto>($"api/client/user/Current/{Id}");
            if (result.IsSuccess)
                return result.Data;
            else return null;
        }

        internal async Task<BZUserDto> FindUserByEmailAsync(string email)
        {
            var result = await httpClient.GetWithJsonResultAsync<BZUserDto>($"api/client/user/FindUserByEmail/{email}");
            if (result.IsSuccess)
                return result.Data;
            else return null;
        }

        /// <summary>
        /// 修改用户基本信息
        /// </summary>
        /// <param name="updateUser"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> UpdateUserAsync(UpdateUserDto updateUser)
        {
            return await HttpRequestWithValidate($"api/client/user/Update", HttpMethod.Patch, updateUser.BuildHttpContent());
        }



        /// <summary>
        /// 修改邮箱
        /// </summary>
        /// <param name="updateUserEmailDto"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> UpdateEmailAsync(UpdateUserEmailDto updateUserEmailDto)
        {
            return await HttpRequestWithValidate($"api/client/user/UpdateEmail", HttpMethod.Patch, updateUserEmailDto.BuildHttpContent());
        }

        internal async Task<bool> IsUserInRole(string roleId, string UserId)
        {
            var result = await httpClient.GetWithJsonResultAsync($"api/client/user/IsUserInRole/{roleId}/{UserId}");
            if (result.IsSuccess)
                return (bool)result.Data;
            else return true;
        }


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> ResetPasswordAsync(UpdateUserPasswordDto resetUserPasswordDto)
        {
            return await HttpRequestWithValidate($"api/client/user/UpdatePassword", HttpMethod.Patch, resetUserPasswordDto.BuildHttpContent());
        }


        /// <summary>
        /// 更新头像
        /// </summary>
        /// <param name="updateUserAvatorDto"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> UpdateUserAvator(UpdateUserAvatorDto updateUserAvatorDto)
        {
            return await HttpRequestWithValidate($"api/client/user/UpdateUserAvator", HttpMethod.Patch, updateUserAvatorDto.BuildHttpContent());

        }

        internal async Task<List<string>> GetRolesAsync(string UserId)
        {
            var result = await httpClient.GetWithJsonResultAsync<List<string>>($"api/client/user/GetRoles/{UserId}");
            if (result.IsSuccess)
                return result.Data;
            else return null;
        }

        internal async Task<string> GetRoleNameById(string roleId)
        {
            var result = await httpClient.GetWithJsonResultAsync($"api/client/user/GetRoleNameById/{roleId}");
            if (result.IsSuccess)
                return result.Data.ToString();
            else return "";
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
        public async Task<BaseResponse> AddTopic(BZTopicDto bZTopicDto, bool Notice)
        {
            HttpContent httpContent = bZTopicDto.BuildHttpContent();
            var notice = Notice ? 1 : 0;
            return await HttpRequestWithValidate($"api/client/Topic/Add/{notice}", HttpMethod.Post, httpContent);
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


            var cachedata = await localStorageService.CreateOrGetCache<VersionCache>("VersionCache", async () =>
            {
                var data = await httpClient.GetWithJsonResultAsync<List<BZVersionDto>>($"api/client/Version/GetAll");
                if (data != null)
                    return new VersionCache
                    {
                        Expire = DateTime.Now.AddDays(1),
                        BzVersionDtos = data.Data.ToList()
                    };
                return new VersionCache { Expire = null, BzVersionDtos = new List<BZVersionDto>() };
            });
            return new BaseResponse<List<BZVersionDto>>
            {
                Code = cachedata.Expire.HasValue ? 200 : 300,
                Data = cachedata.BzVersionDtos,
            };
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
            var tokenJson = await localStorageService.GetItemAsync<string>("authToken");
            if (string.IsNullOrWhiteSpace(tokenJson))
                return null;

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenJson);
            try
            {
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }


        #endregion 处理需要授权才能访问的接口
    }
}