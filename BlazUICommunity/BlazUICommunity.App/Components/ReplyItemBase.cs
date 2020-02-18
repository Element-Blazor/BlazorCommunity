using Blazui.Community.App.Pages;
using Blazui.Community.DTO;
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
        protected bool IsSelefReply { get; set; } = false;

        protected BCard bCard;
        protected BButton bbutton;
        [Parameter]
        public EventCallback<MouseEventArgs> DeleteReply { get; set; }
        protected override async Task InitilizePageDataAsync()
        {
            var user = await GetUser();
            IsSelefReply = (user != null && user.Id == ReplyModel.UserId);
            await Task.CompletedTask;
        }
      
        protected override bool ShouldRender()
        {
            return true;
        }

    }
}
