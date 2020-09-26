using BlazorCommunity.DTO;
using Element;
using Microsoft.AspNetCore.Components;

namespace BlazorCommunity.WasmApp.Pages.Mobile
{
    public partial class MobileReplyItem : ElementComponentBase
    {
        [Parameter]
        public BZReplyDto ReplyModel { get; set; }


        protected override bool ShouldRender() => true;

    
    }
}
