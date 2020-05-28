using Blazui.Community.App.Service;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Pages
{
    public class ReplyTopicBase : PageBase
    {
        [Parameter]
        public string TopicId { get; set; }
        protected BZTopicDto TopicModel { get; set; } = new BZTopicDto();
        protected BZFollowDto FollowModel { get; set; } = new BZFollowDto();

        private string TopicContent;
        private string TopicTitle;
        [Inject]
        private BrowerService browerService { get; set; }
        protected override void OnInitialized()
        {
            if (browerService.IsMobile())
            {
                NavigationManager.NavigateTo($"m/topic/{TopicId}",true);
            }
        }
        /// <summary>
        /// 是否已收藏该帖子
        /// </summary>
        protected bool IsFollow { get; set; } = false;

        /// <summary>
        /// 是否是当前登录用户的帖子
        /// </summary>
        protected bool IsMySelf { get; set; } = false;

        /// <summary>
        /// 是否正在编辑
        /// </summary>
        protected bool IsEditing { get; set; } = false;

        private BZUserModel User;
        [Inject]
        public RoleManager<IdentityRole<string>> roleManager { get; set; }
        protected override async Task InitilizePageDataAsync()
        {
            if (string.IsNullOrWhiteSpace(TopicId))
            {
                ToastError("帖子不存在或已删除");
                NavigationManager.NavigateTo("/");
                return;
            }
            NavigationManager.LocationChanged -= NavigationManager_LocationChanged;
            NavigationManager.LocationChanged += NavigationManager_LocationChanged;
            await WithFullScreenLoading(async () =>
            {
                User = await GetUser();
                await LoadTopic();
                await LoadFollow();
            });
        }

        protected override bool ShouldRender() => true;
        private void NavigationManager_LocationChanged(object sender, Microsoft.AspNetCore.Components.Routing.LocationChangedEventArgs e)
        {
            NavigationManager.NavigateTo(NavigationManager.Uri, true);
        }

        private async Task LoadTopic()
        {
            var result = await NetService.QueryTopicById(TopicId);

            if (result.IsSuccess && result.Data != null)
            {
                if (!string.IsNullOrWhiteSpace(result.Data.RoleId))
                {
                    if (User == null)
                    {
                        ToastWarning("抱歉，该主题设置了权限，请登录后再试");
                        await Task.Delay(1000);
                        NavigationManager.NavigateTo("/");
                        return;
                    }
                    var role = roleManager.Roles.FirstOrDefault(p => p.Id == result.Data.RoleId);
                    var userroles = await userManager.GetRolesAsync(User);
                    var isInRole = userroles.Contains(role.Name);

                    if (!isInRole)
                    {
                        ToastWarning("抱歉，您当前没有权限查看该主题,请联系管理员");
                        await Task.Delay(1000);
                        NavigationManager.NavigateTo("/");
                        return;
                    }
                }
                TopicModel = result.Data;
                TopicModel.Avator ??= "/img/defaultAct.png";
                IsMySelf = TopicModel.CreatorId == User?.Id;
                TopicContent = TopicModel.Content;
                TopicTitle = TopicModel.Title;
                var versions = await QueryVersions();
                TopicModel.VerName = versions?.FirstOrDefault(p => p.Id == TopicModel.VersionId)?.VerName;
            }
            else
            {
                ToastError("帖子不存在或已删除");
                NavigationManager.NavigateTo("/");
                return;
            }
        }


        protected async Task<List<BZVersionDto>> QueryVersions()
        {
            return await memoryCache.GetOrCreateAsync("Version", async p =>
            {
                p.SetSlidingExpiration(TimeSpan.FromMinutes(10));
                var result = await NetService.QueryAllVersions();
                if (result.IsSuccess)
                    return result.Data;
                else return new List<BZVersionDto>();
            });
        }

        /// <summary>
        /// 判断当前用户是否收藏了该帖子
        /// </summary>
        /// <returns></returns>
        private async Task LoadFollow()
        {
            if (User is null)
            {
                IsFollow = false;
                return;
            }
            var result = await NetService.IsFollowed(User.Id, TopicId);
            if (result.IsSuccess && result.Data != null)
            {
                FollowModel = result.Data;
                IsFollow = FollowModel.Status == 0;
            }
        }

        /// <summary>
        /// 回帖删除
        /// </summary>
        /// <returns></returns>
        protected void OnReplyDelete() => TopicModel.ReplyCount--;

        protected void OnReplySuccess() => TopicModel.ReplyCount++;

        /// <summary>
        /// 收藏、取消收藏
        /// </summary>
        /// <returns></returns>
        protected async Task ToggleFollow()
        {
            if (User == null)
            {
                ToastWarning("请登录");
                return;
            }

            if (string.IsNullOrWhiteSpace(FollowModel.Id))
            {
                FollowModel.CreateDate = DateTime.Now;
                FollowModel.Status = 0;
                FollowModel.TopicId = TopicId;
                FollowModel.CreatorId = User.Id;
                FollowModel.LastModifierId = User.Id;
                FollowModel.LastModifyDate = DateTime.Now;
            }

            await WithFullScreenLoading(async () =>
            {
                var result = await NetService.ToggleFollow(FollowModel);

                if (result.IsSuccess)
                {
                    if (string.IsNullOrEmpty(FollowModel.Id))
                    {
                        ToastSuccess("收藏成功");
                    }
                    else
                    {
                        MessageService.Show(FollowModel.Status == -1 ? "收藏成功" : "取消收藏", FollowModel.Status == -1 ? MessageType.Success : MessageType.Info);
                    }
                    await LoadFollow();
                }
            });
        }

        /// <summary>
        /// 切换编辑状态
        /// </summary>
        /// <returns></returns>
        protected async Task ToggleEditing()
        {
            if (IsEditing)
            {
                if (!TopicContent.Equals(TopicModel.Content) || !TopicTitle.Equals(TopicModel.Title))
                {
                    await WithFullScreenLoading(async () =>
                    {
                        var result = await NetService.UpdateTopic(new BZTopicDto() { Id = TopicModel.Id, Content = TopicModel.Content, Title = TopicModel.Title });
                        MessageService.Show(result.IsSuccess ? "编辑成功" : "编辑失败", result.IsSuccess ? MessageType.Success : MessageType.Error);
                        if (result != null && result.IsSuccess)
                            await LoadTopic();
                    });
                }
            }
            IsEditing = !IsEditing;
        }

        protected async Task EndTopic()
        {
            await NetService.EndTopic(TopicModel.Id);
        }

        internal void GoHome() => NavigationManager.NavigateTo("/");
    }
}