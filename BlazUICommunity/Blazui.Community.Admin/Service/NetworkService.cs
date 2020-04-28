using AspectCore.DynamicProxy;
using Blazui.Community.Admin.QueryCondition;
using Blazui.Community.DTO;
using Blazui.Community.DTO.Admin;
using Blazui.Community.HttpClientExtensions;
using Blazui.Community.Response;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Blazui.Community.Admin.Service
{
    public class NetworkService
    {
        private readonly HttpClient httpClient;
        private readonly BaseResponse Unauthorized= 
            new BaseResponse(403, "Unauthorized ，对不起您没有权限进行该操作 ", null);
        private bool isSupper = false;
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
            isSupper = adminUserService.IsSupperAdmin().Result;
        }

        #region User

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <returns></returns>
        public virtual async Task<BaseResponse<PageDatas<UserDisplayDto>>> QueryUsers(QueryUserCondition querycondition, bool MustRefresh = false)
        {
            return await httpClient.GetWithJsonResultAsync<PageDatas<UserDisplayDto>>
                ($"api/user/Query{querycondition.BuildHttpQueryParam(MustRefresh)}");
        }



        /// <summary>
        /// 查询角色Claims
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        internal async Task<BaseResponse<List<RoleClaimDto>>> QueryRoleClaims(string roleId)
        {
            return await httpClient.GetWithJsonResultAsync<List<RoleClaimDto>>
               ($"api/user/QueryRoleClaims/{roleId}");
        }

        internal async Task<BaseResponse> SetUserRoles(UserRoleDto userRoleDto)
        {
            if (!isSupper)
                return Unauthorized;
            return await httpClient.PostWithJsonResultAsync("api/user/AddRolesToUser", userRoleDto.BuildHttpContent());
        }

        internal Task<BaseResponse> DeleteRoleClaim(RoleClaimDto roleClaimDto)
        {
            return  httpClient.PostWithJsonResultAsync
               ($"api/user/DeleteRoleClaim", roleClaimDto.BuildHttpContent());
        }
        internal async Task<BaseResponse<List<string>>> QueryUserRoles(string UserId)
        {
            return await httpClient.GetWithJsonResultAsync<List<string>>
             ($"api/user/QueryUserRoles/{UserId}");
        }

        /// <summary>
        /// 查询角色列表
        /// </summary>
        /// <returns></returns>
        internal async Task<BaseResponse<List<RoleDisplayDto>>> QueryRoles( bool MustRefresh = false)
        {
            return await httpClient.GetWithJsonResultAsync< List<RoleDisplayDto>>
                ($"api/user/QueryRoles");
        }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="roleDto"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> AddRoleAsync(NewRoleDto roleDto)
        {
            if (!isSupper) return Unauthorized;
            return await httpClient.PostWithJsonResultAsync("api/user/CreateRole", roleDto.BuildHttpContent());
        }
        internal async Task<BaseResponse> AddRoleClaimAsync(RoleClaimDto  roleClaimDto)
        {
            if (!isSupper) return Unauthorized;

            return await httpClient.PostWithJsonResultAsync("api/user/CreateRoleClaim", roleClaimDto.BuildHttpContent());
        }

        internal async Task<BaseResponse> UpdateRoleAsync(RoleDisplayDto RoleDto)
        {
            if (!isSupper)return Unauthorized;

            return await httpClient.PostWithJsonResultAsync("api/user/UpdateRole", RoleDto.BuildHttpContent());
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> DeleteRoleAsync(string RoleId)
        {
            if (!isSupper) return Unauthorized;
            return await httpClient.DeleteWithJsonResultAsync($"api/user/DeleteRole/{RoleId}");
        }

        /// <summary>
        /// 封禁账号
        /// </summary>
        /// <returns></returns>
        internal async Task<BaseResponse> DeleteUser(string UserId)
        {
            if (!isSupper) return Unauthorized;
            return await httpClient.PatchWithJsonResultAsync($"api/user/Delete/{UserId}");
        }

        /// <summary>
        /// 解封
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> ResumeUser(string UserId)
        {
            if (!isSupper)
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
            if (!isSupper)return Unauthorized;
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
            if (!isSupper) return Unauthorized;
            return await httpClient.PatchWithJsonResultAsync($"api/Reply/Delete/{Id}");
        }

        /// <summary>
        /// 恢复
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> ResumeReply(string Id)
        {
            if (!isSupper)
                return Unauthorized;
            return await httpClient.PatchWithJsonResultAsync($"api/Reply/Resume/{Id}");
        }

        /// <summary>
        /// 查询回复贴
        /// </summary>
        /// <param name="querycondition"></param>
        /// <returns></returns>
        internal async Task<BaseResponse<PageDatas<ReplyDisplayDto>>> QueryReplys(QueryReplyCondition querycondition, bool MustRefresh = false)
        {
            return await httpClient.GetWithJsonResultAsync<PageDatas<ReplyDisplayDto>>($"api/Reply/Query{querycondition.BuildHttpQueryParam(MustRefresh)}");
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
            if (!isSupper)return Unauthorized;

            return await httpClient.PostWithJsonResultAsync("api/Topic/Add", bZTopicDto.BuildHttpContent());
        }

        /// <summary>
        /// 删除帖子
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> DelTopic(string Id)
        {
            if (!isSupper) return Unauthorized;
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
            if (!isSupper) return Unauthorized;
            return await httpClient.PatchWithJsonResultAsync($"api/Topic/Best/{Id}");
        }

        /// <summary>
        /// 置顶
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> TopTopic(string Id)
        {
            if (!isSupper) return Unauthorized;
            return await httpClient.PatchWithJsonResultAsync($"api/Topic/Top/{Id}");
        }

        /// <summary>
        /// 结贴
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> EndTopic(string Id)
        {
            if (!isSupper) return Unauthorized;
            return await httpClient.PatchWithJsonResultAsync($"api/Topic/End/{Id}");
        }


        internal async Task<BaseResponse> SetAuthorizeToTopic(string topicId, string roleId)
        {
            if (!isSupper) return Unauthorized;
            return await httpClient.PatchWithJsonResultAsync($"api/Topic/authorize/{topicId}/{roleId}");
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
            if (!isSupper) return Unauthorized;
            return await httpClient.PatchWithJsonResultAsync($"api/version/Delete/{Id}");
        }

        /// <summary>
        /// 恢复
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> ResumeVersion(string Id)
        {
            if (!isSupper) return Unauthorized;
            return await httpClient.PatchWithJsonResultAsync($"api/version/Resume/{Id}");
        }

        /// <summary>
        /// 新增版本
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> NewVersion(VersionDisplayDto dto)
        {
            if (!isSupper) return Unauthorized;
            return await httpClient.PostWithJsonResultAsync($"api/version/Add", dto.BuildHttpContent());
        }

        /// <summary>
        /// 更新版本
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> UpdateVersion(VersionDisplayDto dto)
        {
            if (!isSupper)return Unauthorized;
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
            if (!isSupper)  return Unauthorized;
            return await httpClient.PatchWithJsonResultAsync($"api/banner/Delete/{Id}");
        }

        /// <summary>
        /// 删除Banner
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> ResumeBanner(string Id)
        {
            if (!isSupper)   return Unauthorized;
            return await httpClient.PatchWithJsonResultAsync($"api/banner/Resume/{Id}");
        }

        /// <summary>
        /// 发布Banner
        /// </summary>
        /// <param name="bzBannerDto"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> NewBanner(BannerDisplayDto bzBannerDto)
        {
            if (!isSupper)    return Unauthorized;
            return await httpClient.PostWithJsonResultAsync("api/Banner/Add", bzBannerDto.BuildHttpContent());
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="bzBannerDto"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> UpdateBanner(BannerDisplayDto bzBannerDto)
        {
            if (!isSupper)  return Unauthorized;
            HttpContent httpContent = bzBannerDto.BuildHttpContent();
            return await httpClient.PostWithJsonResultAsync("api/Banner/Update", httpContent);
        }

        #endregion Banner
    }
}