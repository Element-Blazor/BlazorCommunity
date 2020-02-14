using Blazui.Community.App.Model;
using Blazui.Community.Utility;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Components
{
    public class TopItemBase : BComponentBase
    {
        [Parameter]
        public TopicItemModel Topic { get; set; }

        [Inject]
        public NavigationManager navigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Topic.ReleaseTime = Convert.ToDateTime(Topic.ReleaseTime).ConvertToDateDiffStr();
        }

        protected void NavigationToReply(int? topicId)
        {
            if (Topic.Status == 1)
            {
                Toast("该帖子已结帖");
                return;
            }
            if (topicId == null || topicId < 0)
                return;
            navigationManager.NavigateTo($"/topic/reply?tpid={topicId}");
        }
    }

}
