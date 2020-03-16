using Blazui.Community.Admin.QueryCondition;
using Blazui.Community.Admin.ViewModel;
using Blazui.Community.DTO;
using Blazui.Community.DTO.Admin;
using Blazui.Community.Request;
using Blazui.Community.Utility.Extensions;
using Blazui.Community.Utility.Response;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using HttpMethod = Blazui.Community.Utility.Extensions.HttpMethod;

namespace Blazui.Community.Admin.Service
{
    public class NetworkService
    {
        private readonly HttpClient httpClient;
        private readonly AdminUserService _adminUserService;
        private readonly BaseResponse Unauthorized;

        private static Dictionary<string, PropertyInfo[]> QuaryParams = new Dictionary<string, PropertyInfo[]>();
        private bool IsSupperRole = false;
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
        /// 构建QueryParam
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        private static string BuildHttpQueryParam<T>(T t, bool MustRefresh = false)
        {
            if (t is null)
                return MustRefresh ? $"MustRefresh={DateTime.Now.Ticks}" : string.Empty;
            var queryparam = string.Empty;
            var queryKey = t.GetType().FullName;
            if (!QuaryParams.TryGetValue(queryKey, out PropertyInfo[] props))
            {
                props = t.GetType().GetProperties();
                QuaryParams.Add(queryKey, props);
            }
            props = props.Where(p => p.GetValue(t) != null).ToArray();
            if (props.Any())
                queryparam = "?";
            foreach (PropertyInfo prop in props)
            {
                var value = IsNullableEnum(prop.PropertyType)? (int)prop.GetValue(t) : prop.GetValue(t);//可空的枚举 如何判断他是枚举??????

                queryparam += $"{prop.Name}={value}&";
            }
            queryparam = queryparam.TrimEnd('&');
            return MustRefresh ? $"{queryparam}&MustRefresh={DateTime.Now.Ticks}" : queryparam;
        }
         static bool IsNullableEnum( Type t)
        {
            Type u = Nullable.GetUnderlyingType(t);
            return (u != null) && u.IsEnum;
        }
        /// <summary>
        /// 查询用户
        /// </summary>
        /// <returns></returns>
        internal async Task<BaseResponse<PageDatas<UserDisplayDto>>> QueryUsers(QueryUserCondition querycondition, bool MustRefresh = false)
        {
            return await httpClient.GetWithJsonResultAsync<PageDatas<UserDisplayDto>>
                ($"api/user/Query{BuildHttpQueryParam(querycondition, MustRefresh)}");
        }

        /// <summary>
        /// 封禁账号
        /// </summary>
        /// <returns></returns>
        internal async Task<BaseResponse> FrozenUser(string UserId)
        {
            if (!(await _adminUserService.IsSupperAdminLogin()))
                return Unauthorized;
            return await httpClient.PatchWithJsonResultAsync($"api/user/Frozen/{UserId}");
        }
        /// <summary>
        /// 解封
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> UnFrozen(string UserId)
        {
            if (!(await _adminUserService.IsSupperAdminLogin()))
                return Unauthorized;
            return await httpClient.PatchWithJsonResultAsync($"api/user/UnFrozen/{UserId}");
        }



        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> ResetPassword(string UserId)
        {
            if (!(await _adminUserService.IsSupperAdminLogin()))
                return Unauthorized;
            return await httpClient.PatchWithJsonResultAsync($"api/user/ResetPassword/{UserId}");
        }
        /// <summary>
        /// 查询帖子
        /// </summary>
        /// <param name="querycondition"></param>
        /// <returns></returns>
        internal async Task<BaseResponse<PageDatas<BZTopicDto>>> QueryTopics(QueryTopicCondition querycondition,bool MustRefresh = false)
        {
            return await httpClient.GetWithJsonResultAsync<PageDatas<BZTopicDto>>($"api/Topic/Query{BuildHttpQueryParam(querycondition, MustRefresh)}");
        }

        /// <summary>
        /// 查询版本
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        internal async Task<BaseResponse<PageDatas<BZVersionDto>>> GetVersions(QueryVersionCondition querycondition, bool MustRefresh = false)
        {
            return await httpClient.GetWithJsonResultAsync<PageDatas<BZVersionDto>>($"api/version/GetPageList{BuildHttpQueryParam(querycondition, MustRefresh)}");
        }


        /// <summary>
        /// 删除版本
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> DeleteVersion(string Id)
        {
            if (!(await _adminUserService.IsSupperAdminLogin()))
                return Unauthorized;
            return await httpClient.GetJsonResultAsync($"api/version/Delete/{Id}", HttpMethod.Delete);
        }

        /// <summary>
        /// 新增版本
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> NewVersion(VersionAutoGenerateColumnsDto dto)
        {
            if (!(await _adminUserService.IsSupperAdminLogin()))
                return Unauthorized;
            HttpContent httpContent = BuildHttpContent(dto);
            return await httpClient.GetJsonResultAsync($"api/version/Add", HttpMethod.Post, httpContent);
        }


        /// <summary>
        /// 更新版本
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> UpdateVersion(VersionAutoGenerateColumnsDto dto)
        {
            if (!(await _adminUserService.IsSupperAdminLogin()))
                return Unauthorized;
            HttpContent httpContent = BuildHttpContent(dto);
            return await httpClient.GetJsonResultAsync($"api/version/Update", HttpMethod.Post, httpContent);
        }

        /// <summary>
        /// 查询回复贴
        /// </summary>
        /// <param name="querycondition"></param>
        /// <returns></returns>
        internal async Task<BaseResponse<PageDatas<BZReplyDto>>> QueryReplys(QueryReplyCondition querycondition,bool MustRefresh=false)
        {
            //var url = "api/Reply/QueryReplys";
            //if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(title))
            //{
            //    url += $"?username={username}";
            //    url += $"&topicTitle={title}";
            //}
            //else
            //{
            //    if (!string.IsNullOrWhiteSpace(username))
            //    {
            //        url += $"?username={username}";
            //    }
            //    if (!string.IsNullOrWhiteSpace(title))
            //    {
            //        url += $"?topicTitle={title}";
            //    }
            //}
            return await httpClient.GetWithJsonResultAsync<PageDatas<BZReplyDto>>($"api/Reply/Query{BuildHttpQueryParam(querycondition,MustRefresh)}");
        }


        /// <summary>
        /// 发表帖子
        /// </summary>
        /// <param name="bZTopicDto"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> NewTopic(BZTopicDto bZTopicDto)
        {
            if (!(await _adminUserService.IsSupperAdminLogin()))
                return Unauthorized;
            HttpContent httpContent = BuildHttpContent(bZTopicDto);

            return await httpClient.GetJsonResultAsync("api/Topic/Add", HttpMethod.Post, httpContent);
        }



        /// <summary>
        /// 删除帖子
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> DelTopic(string Id, int status)
        {
            if (!(await _adminUserService.IsSupperAdminLogin()))
                return Unauthorized;
            if (status == -1)
                return await httpClient.GetJsonResultAsync($"api/Topic/Active/{Id}", HttpMethod.Delete);
            return await httpClient.GetJsonResultAsync($"api/Topic/Delete/{Id}", HttpMethod.Delete);
        }

        /// <summary>
        /// 加精
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> BestTopic(string Id)
        {
            if (!(await _adminUserService.IsSupperAdminLogin()))
                return Unauthorized;
            return await httpClient.GetJsonResultAsync($"api/Topic/BestTopic/{Id}");
        }
        /// <summary>
        /// 置顶
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> TopTopic(string Id)
        {
            if (!(await _adminUserService.IsSupperAdminLogin()))
                return Unauthorized;
            return await httpClient.GetJsonResultAsync($"api/Topic/TopTopic/{Id}");
        }

        /// <summary>
        /// 结贴
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> EndTopic(string Id)
        {
            if (!(await _adminUserService.IsSupperAdminLogin()))
                return Unauthorized;
            return await httpClient.GetJsonResultAsync($"api/Topic/EndTopic/{Id}");
        }

        /// <summary>
        /// 删除或恢复回帖
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> DelReply(string Id)
        {
            if (!(await _adminUserService.IsSupperAdminLogin()))
                return Unauthorized;
            return await httpClient.GetJsonResultAsync($"api/Reply/DeleteOrActive/{Id}", HttpMethod.Delete);
        }


        /// <summary>
        /// 删除Banner
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> DeleteBanner(string Id)
        {
            if (!(await _adminUserService.IsSupperAdminLogin()))
                return Unauthorized;
            return await httpClient.GetJsonResultAsync($"api/banner/Delete/{Id}", HttpMethod.Delete);
        }

        /// <summary>
        /// 查询Banner
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        internal async Task<BaseResponse<PageDatas<BzBannerDto>>> GetBanners(int PageSize,int PageIndex)
        {
            return await httpClient.GetJsonResultAsync<PageDatas<BzBannerDto>>($"api/banner/GetPageList/{PageSize}/{PageIndex}");
        }


        /// <summary>
        /// 发布Banner
        /// </summary>
        /// <param name="bzBannerDto"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> NewBanner(BzBannerDto bzBannerDto)
        {
            if (!(await _adminUserService.IsSupperAdminLogin()))
                return Unauthorized;
            HttpContent httpContent = BuildHttpContent(bzBannerDto);
            return await httpClient.GetJsonResultAsync("api/Banner/Add", HttpMethod.Post, httpContent);
        }

        /// <summary>
        /// 发布Banner
        /// </summary>
        /// <param name="bzBannerDto"></param>
        /// <returns></returns>
        internal async Task<BaseResponse> UpdateBanner(BzBannerDto bzBannerDto)
        {
            if (!(await _adminUserService.IsSupperAdminLogin()))
                return Unauthorized;
            HttpContent httpContent = BuildHttpContent(bzBannerDto);
            return await httpClient.GetJsonResultAsync("api/Banner/Update", HttpMethod.Post, httpContent);
        }
    }
}
