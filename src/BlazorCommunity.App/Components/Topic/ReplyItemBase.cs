using BlazorCommunity.DTO;
using BlazorCommunity.Enums;
using Element;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

namespace BlazorCommunity.App.Components
{
    public class ReplyItemBase : ElementComponentBase
    {
        [Parameter]
        public BZReplyDto ReplyModel { get; set; }

        [Parameter]
        public EventCallback OnDeleted { get; set; }
        [Parameter]
        public EventCallback ReferenceReply { get; set; }
        [Parameter]
        public bool IsTopicEnd { get; set; }

        [Parameter]
        public EventCallback OnEdit { get; set; }

        [Inject]
        internal IConfiguration Configuration { get; set; }

        internal string UploadUrl { get; private set; }

        protected override bool ShouldRender() => true;

        protected override void OnInitialized()
        {
            UploadUrl = Configuration["ServerUrl"] + "/api/upload/" + UploadPath.Topic.Description();
        }
    }
}