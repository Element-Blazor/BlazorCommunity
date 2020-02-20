using Blazui.Community.App.Pages;
using Blazui.Community.DTO;
using Blazui.Component;
using Blazui.Component.Button;
using Blazui.Component.Container;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;

namespace Blazui.Community.App.Components
{
    public class ReplyItemBase : PageBase
    {
        [Parameter]
        public BZReplyDtoWithUser ReplyModel { get; set; }

        protected BCard bCard;

        protected BButton bbutton;
        [Parameter]
        public EventCallback<MouseEventArgs> DeleteReply { get; set; }



        protected override async Task InitilizePageDataAsync()
        {
            Content = ReplyModel.Content;
            var user = await GetUser();
          IsSelfReply = user != null && user.Id == ReplyModel.UserId;
            await Task.CompletedTask;
        }



        protected async Task EditReply()
        {
            if (ShoudEdit)
            {
                if (Content != ReplyModel.Content)
                {
                    var result = await NetService.UpdateReply(new BZReplyDto() { Id = ReplyModel.Id, Content = ReplyModel.Content });
                    if (result != null)
                        MessageService.Show(result.IsSuccess ? "编辑成功" : "编辑失败", result.IsSuccess ? MessageType.Success : MessageType.Error);
                }
            }
            ShoudEdit = !ShoudEdit;

            bCard?.Refresh();
            this.StateHasChanged();

        }
        protected override bool ShouldRender() => true;

        protected bool IsSelfReply = false;

        internal bool ShoudEdit = false;
        ///// <summary>
        ///// 是否需要编辑
        ///// </summary>
        //[Parameter]
        //public bool ShoudEdit { get; set; } = false;
        /// <summary>
        /// 原始内容
        /// </summary>
        protected string Content { get; set; }
    }
}
