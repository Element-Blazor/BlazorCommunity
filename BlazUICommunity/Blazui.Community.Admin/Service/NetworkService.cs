using Blazui.Community.Admin.QueryCondition;
using Blazui.Community.Admin.ViewModel;
using Blazui.Community.DTO;
using Blazui.Community.Request;
using Blazui.Community.Utility.Extensions;
using Blazui.Community.Utility.Response;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using HttpMethod = Blazui.Community.Utility.Extensions.HttpMethod;

namespace Blazui.Community.Admin.Service
{
    public class NetworkService
    {
        private readonly HttpClient httpClient;
        private readonly AdminUserService _adminUserService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AuthenticationStateProvider _AuthenticationStateProvider;
        private readonly BaseResponse Unauthorized;
        private bool IsSupperRole = false;
        public NetworkService(IHttpClientFactory httpClientFactory, AdminUserService adminUserService)
        {
            this.httpClient = httpClientFactory.CreateClient("BlazuiCommunitiyAdmin");
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
        /// 查询用户
        /// </summary>
        /// <returns></returns>
        internal async Task<BaseResponse<PageDatas<BZUserDto>>> QueryUsers(QueryUserCondition querycondition)
        {
            return await httpClient.PostJsonAsync<PageDatas<BZUserDto>>("api/user/Query", BuildHttpContent(querycondition));
        }
        /// <summary>
        /// 查询用户
        /// </summary>
        /// <returns></returns>
        internal async Task<BaseResponse<BZUserDto>> QueryUserByUserName(string UserName)
        {
            return await httpClient.GetJsonAsync<BZUserDto>($"api/user/QueryByName/{UserName}");
        }
        /// <summary>
        /// 封禁账号
        /// </summary>
        /// <returns></returns>
        internal async Task<BaseResponse> FrozenUser(string UserId)
        {
            if (!(await _adminUserService.IsSupperAdminLogin()))
                return Unauthorized;
            return await httpClient.GetJsonAsync($"api/user/Frozen/{UserId}");
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
            return await httpClient.GetJsonAsync($"api/user/UnFrozen/{UserId}");
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
            return await httpClient.GetJsonAsync($"api/user/ResetPassword/{UserId}");
        }
        /// <summary>
        /// 查询帖子
        /// </summary>
        /// <param name="querycondition"></param>
        /// <returns></returns>
        internal async Task<BaseResponse<PageDatas<BZTopicDto>>> QueryTopics(QueryTopicCondition querycondition, string username = "")
        {
            var url = "api/Topic/Query";
            if (!string.IsNullOrWhiteSpace(username))
                url += $"?username={username}";
            return await httpClient.PostJsonAsync<PageDatas<BZTopicDto>>(url, BuildHttpContent(querycondition));
        }

        /// <summary>
        /// 查询版本
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        internal async Task<BaseResponse<PageDatas<BZVersionDto>>> GetVersions(PageInfo pageInfo, int projectId = -1)
        {
            return await httpClient.GetJsonAsync<PageDatas<BZVersionDto>>($"api/version/GetPageList/{projectId}/{pageInfo.PageSize}/{pageInfo.PageIndex}");
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
            return await httpClient.GetJsonAsync($"api/version/Delete/{Id}", HttpMethod.Delete);
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
            return await httpClient.PostJsonAsync($"api/version/Add", httpContent);
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
            return await httpClient.PostJsonAsync($"api/version/Update", httpContent);
        }

        /// <summary>
        /// 查询回复贴
        /// </summary>
        /// <param name="querycondition"></param>
        /// <returns></returns>
        internal async Task<BaseResponse<PageDatas<BZReplyDto>>> QueryReplys(QueryReplyCondition querycondition, string username, string title)
        {
            var url = "api/Reply/QueryReplys";
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(title))
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
            return await httpClient.PostJsonAsync<PageDatas<BZReplyDto>>(url, BuildHttpContent(querycondition));
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

            return await httpClient.PostJsonAsync("api/Topic/Add", httpContent);
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
                return await httpClient.GetJsonAsync($"api/Topic/Active/{Id}", HttpMethod.Delete);
            return await httpClient.GetJsonAsync($"api/Topic/Delete/{Id}", HttpMethod.Delete);
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
            return await httpClient.GetJsonAsync($"api/Topic/BestTopic/{Id}");
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
            return await httpClient.GetJsonAsync($"api/Topic/TopTopic/{Id}");
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
            return await httpClient.GetJsonAsync($"api/Topic/EndTopic/{Id}");
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
            return await httpClient.GetJsonAsync($"api/Reply/DeleteOrActive/{Id}", HttpMethod.Delete);
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
            return await httpClient.GetJsonAsync($"api/banner/Delete/{Id}", HttpMethod.Delete);
        }

        /// <summary>
        /// 查询Banner
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        internal async Task<BaseResponse<PageDatas<BzBannerDto>>> GetBanners(PageInfo pageInfo)
        {
            return await httpClient.GetJsonAsync<PageDatas<BzBannerDto>>($"api/banner/GetPageList/{pageInfo.PageSize}/{pageInfo.PageIndex}");
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
            return await httpClient.PostJsonAsync("api/Banner/Add", httpContent);
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
            return await httpClient.PostJsonAsync("api/Banner/Update", httpContent);
        }
    }
}
