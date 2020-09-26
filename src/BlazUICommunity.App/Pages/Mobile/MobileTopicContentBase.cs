using Blazui.Community.DTO;
using Blazui.Community.Enums;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Pages.Mobile
{
    public class MobileTopicContentBase : BComponentBase
    {

        protected string tagClass = "el-tag--success";
        [Parameter]
        public BZTopicDto TopicModel { get; set; }

        /// <summary>
        /// 是否已收藏该帖子
        /// </summary>
        [Parameter]
        public bool IsFollow { get; set; }

        /// <summary>
        /// 主贴收藏/取消收藏状态改变事件
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnFollowStatusChanging { get; set; }


        /// <summary>
        ///
        /// </summary>
        /// <param name="eventArgs"></param>
        /// <returns></returns>
        protected async Task ToggleStarStatus(MouseEventArgs eventArgs)
        {
            if (OnFollowStatusChanging.HasDelegate)
            {
                await OnFollowStatusChanging.InvokeAsync(eventArgs);
                StateHasChanged();
            }
        }

        protected void GoBack()
        {
            NavigationManager.NavigateTo("/m/index/",true);
        }
        protected override void OnInitialized()
        {
            if (TopicModel != null)
            {
                switch ((TopicCategory)TopicModel.Category)
                {
                    case TopicCategory.Ask:
                        tagClass = ConstConfig.TagClasses[3];
                        break;
                    case TopicCategory.Share:
                        tagClass = ConstConfig.TagClasses[1];
                        break;
                    case TopicCategory.Discuss:
                        tagClass = ConstConfig.TagClasses[2];
                        break;
                    case TopicCategory.Suggest:
                        tagClass = ConstConfig.TagClasses[2];
                        break;
                    case TopicCategory.Notice:
                        tagClass = ConstConfig.TagClasses[0];
                        break;
                    default:
                        break;
                }
            }
        }
        protected override bool ShouldRender() => true;
    }
}
