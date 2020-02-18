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

        public bool IsStar = false;
        protected BCard bCard;
        protected BZFollowModel follow;
        protected override async Task InitilizePageDataAsync()
        {
            await Task.CompletedTask;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (!firstRender)
                return;
            if (TopicId > 0)
            {
                var result = await NetService.GetTopicById(TopicId);
                if (result.IsSuccess&&result.Data!=null)
                    TopicModel = mapper.Map<TopicItemModel>(result.Data);
                TopicModel.ReleaseTime = result.Data.PublishTime.ConvertToDateDiffStr();
                var user = await GetUser();
                if (user != null)
                {
                    var response = await NetService.IsStar(user.Id, TopicId);
                    follow = response?.Data;
                    IsStar = follow != null && follow.Status == 0;
                }
                UpdateUI();
            }
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
                MessageService.Show("请先登录", Component.MessageType.Warning);
                return;
            }
            BZFollowDto dto = new BZFollowDto() { };
            if (follow != null)
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
                MessageService.Show(IsStar ? "收藏成功" : "取消收藏", IsStar? Component.MessageType.Success:Component.MessageType.Info);
                UpdateUI();
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
