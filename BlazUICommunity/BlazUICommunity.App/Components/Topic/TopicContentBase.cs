using Blazui.Community.App.Model;
using Blazui.Community.App.Pages;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using Blazui.Community.Utility;
using Blazui.Component;
using Blazui.Component.Container;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Blazui.Community.Enums;

namespace Blazui.Community.App.Components
{
    public class TopicContentBase : BComponentBase
    {
        [Inject]
        IConfiguration Configuration { get; set; }
        [Parameter]
        public BZTopicDto TopicModel { get; set; }

       
        /// <summary>
        /// 是否已收藏该帖子
        /// </summary>
        [Parameter]
        public bool IsFollow { get; set; }

        /// <summary>
        /// 是否是当前登录用户的帖子
        /// </summary>
        [Parameter]
        public bool IsMySelf { get; set; }
        /// <summary>
        /// 是否正在编辑
        /// </summary>
        [Parameter]
        public bool IsEditing { get; set; }

        
        /// <summary>
        /// 主贴编辑状态改变事件
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnEditStatusChanging { get; set; }

      
        /// <summary>
        /// 主贴收藏/取消收藏状态改变事件
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnFollowStatusChanging { get; set; }

        internal string UploadUrl { get; set; }
        protected override void OnInitialized()
        {
            base.OnInitialized();
            UploadUrl = Configuration["ServerUrl"] + "/api/upload/" + UploadPath.Topic.Description();
        }

        protected async Task ToggleEditStatus(MouseEventArgs args)
        {

            if (OnEditStatusChanging.HasDelegate)
            {
                await OnEditStatusChanging.InvokeAsync(args);
                StateHasChanged();
            }

        }


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

        protected override bool ShouldRender() => true;


    }
}
