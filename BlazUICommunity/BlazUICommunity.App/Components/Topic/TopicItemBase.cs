using Blazui.Community.App.Model;
using Blazui.Community.DTO;
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
    public class TopicItemBase : BComponentBase
    {
        [Parameter]
        public  BZTopicDto Topic { get; set; }

        [Inject]
        public NavigationManager navigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        protected void NavigationToReply(string topicId)
        {
            if (Topic.Status == 1)
            {
                Toast("该帖子已结帖");
                return;
            }
            if (topicId == null || string.IsNullOrWhiteSpace(topicId))
                return;
            navigationManager.NavigateTo($"/topic/{topicId}");
        }
    }

}
