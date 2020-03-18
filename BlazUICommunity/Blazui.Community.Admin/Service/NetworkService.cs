using Blazui.Community.Admin.QueryCondition;
using Blazui.Community.DTO;
using Blazui.Community.DTO.Admin;
using Blazui.Community.Response;
using Blazui.Community.HttpClientExtensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace Blazui.Community.Admin.Service
{
    public class NetworkService
    {
        private readonly HttpClient httpClient;
        private readonly AdminUserService _adminUserService;
        private readonly BaseResponse Unauthorized;



        public NetworkService(IHttpClientFactory httpClientFactory, AdminUserService adminUserService)
        {
            this.httpClient = httpClientFactory.CreateClient("BlazuiCommunitiyAdmin");
            httpClient.DefaultRequestHeaders.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue()
            {
                NoCache = false,
                NoStore = false,
                MaxAge = TimeSpan.FromSeconds(0),
                MustRevalidate = false,
                Public = false,
            };
            _adminUserService = adminUserService;
            Unauthorized = new BaseResponse(403, "Unauthorized ，对不起您没有权限进行该操作 ", null);
        }





        #region User

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <returns></returns>
        internal async Task<BaseResponse<PageDatas<UserDisplayDto>>> QueryUsers(QueryUserCondition querycondition, bool MustRefresh = false)
        {
            return await httpClient.GetWithJsonResultAsync<PageDatas<UserDisplayDto>>
                ($"api/user/Query{querycondition.BuildHttpQueryParam(MustRefresh)}");
        }

        /// <summary>
        /// 封禁账号
        /// </summary>
        /// <returns></returns>
        internal async Task<BaseResponse> DeleteUser(string UserId)
        {
            if (!(await _adminUserService.IsSupperAdmin()))
                return Unauthorized;
            return await httpClient.PatchWithJsonResultAsync($"api/user/Delete/{UserId}");
        }

        /// <summary>
        /// 解封
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> ResumeUser(string UserId)
        {
            if (!(await _adminUserService.IsSupperAdmin()))
                return Unauthorized;
            return await httpClient.PatchWithJsonResultAsync($"api/user/Resume/{UserId}");
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> ResetPassword(string UserId)
        {
            if (!(await _adminUserService.IsSupperAdmin()))
                return Unauthorized;
            return await httpClient.PatchWithJsonResultAsync($"api/user/ResetPassword/{UserId}");
        }

        #endregion User

        #region Reply

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> DeleteReply(string Id)
        {
            if (!(await _adminUserService.IsSupperAdmin()))
                return Unauthorized;
            return await httpClient.PatchWithJsonResultAsync($"api/Reply/Delete/{Id}");
        }

        /// <summary>
        /// 恢复
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> ResumeReply(string Id)
        {
            if (!(await _adminUserService.IsSupperAdmin()))
                return Unauthorized;
            return await httpClient.PatchWithJsonResultAsync($"api/Reply/Resume/{Id}");
        }

        /// <summary>
        /// 查询回复贴
        /// </summary>
        /// <param name="querycondition"></param>
        /// <returns></returns>
        internal async Task<BaseResponse<PageDatas<BZReplyDto>>> QueryReplys(QueryReplyCondition querycondition, bool MustRefresh = false)
        {
            return await httpClient.GetWithJsonResultAsync<PageDatas<BZReplyDto>>($"api/Reply/Query{querycondition.BuildHttpQueryParam(MustRefresh)}");
        }

        #endregion Reply

        #region Topic

        /// <summary>
        /// 查询帖子
        /// </summary>
        /// <param name="querycondition"></param>
        /// <returns></returns>
        internal async Task<BaseResponse<PageDatas<TopicDisplayDto>>> QueryTopics(QueryTopicCondition querycondition, bool MustRefresh = false)
        {
            return await httpClient.GetWithJsonResultAsync<PageDatas<TopicDisplayDto>>($"api/Topic/Query{querycondition.BuildHttpQueryParam(MustRefresh)}");
        }

        /// <summary>
        /// 发表帖子
        /// </summary>
        /// <param name="bZTopicDto"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> NewTopic(BZTopicDto bZTopicDto)
        {
            if (!(await _adminUserService.IsSupperAdmin()))
                return Unauthorized;

            return await httpClient.PostWithJsonResultAsync("api/Topic/Add", bZTopicDto.BuildHttpContent());
        }

        /// <summary>
        /// 删除帖子
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> DelTopic(string Id)
        {
            if (!(await _adminUserService.IsSupperAdmin()))
                return Unauthorized;
            return await httpClient.PatchWithJsonResultAsync($"api/Topic/Delete/{Id}");
        }

        /// <summary>
        /// 恢复帖子
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> ResumeTopic(string Id)
        {
            return await httpClient.PatchWithJsonResultAsync($"api/Topic/Resume/{Id}");
        }

        /// <summary>
        /// 加精
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> BestTopic(string Id)
        {
            if (!(await _adminUserService.IsSupperAdmin()))
                return Unauthorized;
            return await httpClient.PatchWithJsonResultAsync($"api/Topic/Best/{Id}");
        }

        /// <summary>
        /// 置顶
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> TopTopic(string Id)
        {
            if (!(await _adminUserService.IsSupperAdmin()))
                return Unauthorized;
            return await httpClient.PatchWithJsonResultAsync($"api/Topic/Top/{Id}");
        }

        /// <summary>
        /// 结贴
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> EndTopic(string Id)
        {
            if (!(await _adminUserService.IsSupperAdmin()))
                return Unauthorized;
            return await httpClient.PatchWithJsonResultAsync($"api/Topic/End/{Id}");
        }

        #endregion Topic

        #region Version

        /// <summary>
        /// 查询版本
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        internal async Task<BaseResponse<PageDatas<VersionDisplayDto>>> QueryVersions(QueryVersionCondition querycondition, bool MustRefresh = false)
        {
            return await httpClient.GetWithJsonResultAsync<PageDatas<VersionDisplayDto>>($"api/version/Query{querycondition.BuildHttpQueryParam(MustRefresh)}");
        }

        /// <summary>
        /// 删除版本
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> DeleteVersion(string Id)
        {
            if (!(await _adminUserService.IsSupperAdmin()))
                return Unauthorized;
            return await httpClient.PatchWithJsonResultAsync($"api/version/Delete/{Id}");
        }

        /// <summary>
        /// 恢复
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> ResumeVersion(string Id)
        {
            if (!(await _adminUserService.IsSupperAdmin()))
                return Unauthorized;
            return await httpClient.PatchWithJsonResultAsync($"api/version/Resume/{Id}");
        }

        /// <summary>
        /// 新增版本
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> NewVersion(VersionDisplayDto dto)
        {
            if (!(await _adminUserService.IsSupperAdmin()))
                return Unauthorized;
            return await httpClient.PostWithJsonResultAsync($"api/version/Add", dto.BuildHttpContent());
        }

        /// <summary>
        /// 更新版本
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> UpdateVersion(VersionDisplayDto dto)
        {
            if (!(await _adminUserService.IsSupperAdmin()))
                return Unauthorized;
            return await httpClient.PutWithJsonResultAsync($"api/version/Update", dto.BuildHttpContent());
        }

        #endregion Version

        #region Banner

        /// <summary>
        /// 查询Banner
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        internal async Task<BaseResponse<PageDatas<BannerDisplayDto>>> QueryBanners(QueryBannerCondition queryBannerCondition, bool MustRefresh = false)
        {
            return await httpClient.GetWithJsonResultAsync<PageDatas<BannerDisplayDto>>($"api/banner/Query{queryBannerCondition.BuildHttpQueryParam(MustRefresh)}");
        }

        /// <summary>
        /// 删除Banner
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> DeleteBanner(string Id)
        {
            if (!(await _adminUserService.IsSupperAdmin()))
                return Unauthorized;
            return await httpClient.PatchWithJsonResultAsync($"api/banner/Delete/{Id}");
        }

        /// <summary>
        /// 删除Banner
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> ResumeBanner(string Id)
        {
            if (!(await _adminUserService.IsSupperAdmin()))
                return Unauthorized;
            return await httpClient.PatchWithJsonResultAsync($"api/banner/Resume/{Id}");
        }

        /// <summary>
        /// 发布Banner
        /// </summary>
        /// <param name="bzBannerDto"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> NewBanner(BannerDisplayDto bzBannerDto)
        {
            if (!(await _adminUserService.IsSupperAdmin()))
                return Unauthorized;
            return await httpClient.PostWithJsonResultAsync("api/Banner/Add", bzBannerDto.BuildHttpContent());
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="bzBannerDto"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> UpdateBanner(BannerDisplayDto bzBannerDto)
        {
            if (!(await _adminUserService.IsSupperAdmin()))
                return Unauthorized;
            HttpContent httpContent = bzBannerDto.BuildHttpContent();
            return await httpClient.PostWithJsonResultAsync("api/Banner/Update", httpContent);
        }

        #endregion Banner
    }
}