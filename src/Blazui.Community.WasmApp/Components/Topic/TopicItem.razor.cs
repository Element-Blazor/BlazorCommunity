using Blazui.Community.DTO;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Blazui.Community.WasmApp.Components.Topic
{
    public partial class TopicItem : BComponentBase
    {
        [Parameter]
        public BZTopicDto Topic { get; set; }

        [Inject]
        public NavigationManager navigationManager { get; set; }

      

        protected void NavigationToReply(string topicId)
        {
            if (topicId == null || string.IsNullOrWhiteSpace(topicId))
                return;
            NavigationManager.NavigateTo($"/topic/{topicId}");
        }
    }
}