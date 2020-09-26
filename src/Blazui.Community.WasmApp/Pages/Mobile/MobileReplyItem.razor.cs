using Blazui.Community.DTO;
using Blazui.Component;
using Microsoft.AspNetCore.Components;

namespace Blazui.Community.WasmApp.Pages.Mobile
{
    public partial class MobileReplyItem : BComponentBase
    {
        [Parameter]
        public BZReplyDto ReplyModel { get; set; }


        protected override bool ShouldRender() => true;

    
    }
}
