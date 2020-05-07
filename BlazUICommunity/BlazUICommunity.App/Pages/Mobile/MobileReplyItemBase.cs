using Blazui.Community.DTO;
using Blazui.Component;
using Microsoft.AspNetCore.Components;

namespace Blazui.Community.App.Pages.Mobile
{
    public class MobileReplyItemBase : BComponentBase
    {
        [Parameter]
        public BZReplyDto ReplyModel { get; set; }


        protected override bool ShouldRender() => true;

    
    }
}
