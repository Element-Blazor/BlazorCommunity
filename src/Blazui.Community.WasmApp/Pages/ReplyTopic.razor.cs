﻿using Blazui.Community.DTO;
using Blazui.Community.WasmApp.Components.Topic;
using Blazui.Community.WasmApp.Service;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.WasmApp.Pages
{
    public partial class ReplyTopic : PageBase
    {
        [Parameter]
        public string TopicId { get; set; }

        protected BZTopicDto TopicModel { get; set; } = new BZTopicDto();
        protected BZFollowDto FollowModel { get; set; } = new BZFollowDto();

        protected TopicContent mTopicContent;
        private string TopicContent;
        private string TopicTitle;
        [Inject]
        private BrowerService browerService { get; set; }
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

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                if (await browerService.IsMobile())
                {
                    NavigationManager.NavigateTo($"m/topic/{TopicId}", true);
                }
            }
        }
        private async Task LoadTopic()
        {
            var result = await NetService.QueryTopicById(TopicId);
            if (result.IsSuccess && result.Data != null)
            {
                if (!string.IsNullOrWhiteSpace(result.Data.RoleId))
                {
                    var roleId = result.Data.RoleId;
                    if (User == null)
                    {
                        ToastWarning("抱歉，该主题设置了权限，请登录后再试");
                        await Task.Delay(1000);
                        NavigationManager.NavigateTo("/");
                        return;
                    }
                    var IsInRole = await NetService.IsUserInRole(roleId, User.Id);
                    if (!IsInRole)
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
                await Task.Delay(1000);
                NavigationManager.NavigateTo("/");
                await Task.CompletedTask;
            }
        }


        protected async Task<List<BZVersionDto>> QueryVersions()
        {
            var result = await NetService.QueryAllVersions();
            if (result.IsSuccess)
                return result.Data;
            else return new List<BZVersionDto>();
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
           var result= await NetService.EndTopic(TopicModel.Id);
            if (result.IsSuccess)
            {
                ToastSuccess("帖子已关闭");
                NavigationManager.NavigateTo($"topic/{TopicId}", true);
            }
        }

    }
}