using Blazui.Community.App.Model;
using Blazui.Community.App.Pages;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using Blazui.Community.Utility;
using Blazui.Component.Container;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Components
{
    public class TopicContentBase : PageBase
    {
        [Parameter]
        public int TopicId { get; set; }
        protected TopicItemModel TopicModel { get; set; }
        protected string projectName = "";
        /// <summary>
        /// 是否已收藏该帖子
        /// </summary>
        public bool IsStar = false;
        protected BCard bCard;
        protected BZFollowModel follow;
        internal string Content = "";
        protected override async Task InitilizePageDataAsync()
        {
            await Task.CompletedTask;
        }
        /// <summary>
        /// 是否是当前登录用户的帖子
        /// </summary>
        internal bool IsSelf = false;
        internal bool ShoudEdit = false;
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (!firstRender)
                return;
            await LoadData();
        }

        private async Task LoadData()
        {
            if (TopicId > 0)
            {
                var result = await NetService.GetTopicById(TopicId);
                if (result.IsSuccess && result.Data != null)
                {
                    projectName = (await QueryVersions()).FirstOrDefault(p => p.Id == result.Data.versionId)?.VerName;
                    TopicModel = mapper.Map<TopicItemModel>(result.Data);
                    TopicModel.ReleaseTime = result.Data.PublishTime.ConvertToDateDiffStr();
                    Content = TopicModel.Content;
                    var user = await GetUser();
                    if (user != null)
                    {
                        IsSelf = result.Data.UserId == user.Id;
                        var response = await NetService.IsStar(user.Id, TopicId);
                        follow = response?.Data;
                        IsStar = follow != null && follow.Status == 0;
                    }
                    UpdateUI();
                }
                else
                {
                    TopicModel = new TopicItemModel();
                }

            }
        }

        protected async Task ToggleEditStatus()
        {
            if (ShoudEdit)
            {
                if (Content != TopicModel.Content)
                {
                    var result = await NetService.UpdateTopic(new BZTopicDto() { Id = TopicModel.Id, Content = TopicModel.Content });
                    if (result != null)
                        MessageService.Show(result.IsSuccess ? "编辑成功" : "编辑失败", result.IsSuccess?Component.MessageType.Success:Component.MessageType.Error);
                }

            }
            ShoudEdit = !ShoudEdit;

            UpdateUI();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventArgs"></param>
        /// <returns></returns>
        protected async Task ToggleStar(MouseEventArgs eventArgs)
        {
            var user = await GetUser();
            if (user == null)
            {
                ToastWarning("请先登录");
                return;
            }
            BZFollowDto dto = new BZFollowDto() { };
            if (follow != null&& follow.Id>0)
            {
                follow.Status = follow.Status == -1 ? 0 : -1;
                dto = mapper.Map<BZFollowDto>(follow);
            }
            else
            {
                dto.FollowTime = DateTime.Now;
                dto.Status = 0;
                dto.TopicId = TopicId;
                dto.UserId = user.Id;
            }
            var result = await NetService.ToggleFollow(dto);

            if (result.IsSuccess)
            {
                IsStar = dto.Status == 0;
                MessageService.Show(IsStar ? "收藏成功" : "取消收藏", IsStar ? Component.MessageType.Success : Component.MessageType.Info);
                await LoadData();
            }
        }

        protected override bool ShouldRender()
        {
            return true;
        }

        protected void UpdateUI()
        {
            bCard?.Refresh();
            MarkAsRequireRender();
            StateHasChanged();
        }
    }
}
