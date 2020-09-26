using Blazui.Community.DTO;
using Blazui.Community.Enums;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Blazui.Community.WasmApp.Components.Topic
{
    public partial class TopicContent : BComponentBase
    {
        [Inject]
        private IConfiguration Configuration { get; set; }

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
        /// 关闭帖子
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> CloseTopic { get; set; }
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

        protected async Task CloseMyTopic(MouseEventArgs args)
        {
            if (CloseTopic.HasDelegate)
            {
                await CloseTopic.InvokeAsync(args);
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