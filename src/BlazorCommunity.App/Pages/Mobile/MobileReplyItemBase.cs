using BlazorCommunity.DTO;
using Element;
using Microsoft.AspNetCore.Components;

namespace BlazorCommunity.App.Pages.Mobile
{
    public class MobileReplyItemBase : ElementComponentBase
    {
        [Parameter]
        public BZReplyDto ReplyModel { get; set; }


        protected override bool ShouldRender() => true;

    
    }
}
