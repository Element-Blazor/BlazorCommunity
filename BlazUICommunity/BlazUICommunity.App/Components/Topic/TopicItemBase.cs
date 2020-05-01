using Blazui.Community.DTO;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Blazui.Community.App.Components
{
    public class TopicItemBase : BComponentBase
    {
        [Parameter]
        public BZTopicDto Topic { get; set; }

        [Inject]
        public NavigationManager navigationManager { get; set; }

      

        protected void NavigationToReply(string topicId)
        {
            //if (Topic.Status == 1)
            //{
            //    Toast("该帖子已结帖");
            //    return;
            //}
            if (topicId == null || string.IsNullOrWhiteSpace(topicId))
                return;
            NavigationManager.NavigateTo($"/topic/{topicId}");
        }
    }
}