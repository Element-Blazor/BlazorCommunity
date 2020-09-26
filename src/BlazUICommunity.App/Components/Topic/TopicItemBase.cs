using Blazui.Community.DTO;
using Blazui.Community.Enums;
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

        protected string tagClass = "el-tag--success";

        protected override void OnInitialized()
        {
            if (Topic != null)
            {
                switch ((TopicCategory)Topic.Category)
                {
                    case TopicCategory.Ask:
                        tagClass = ConstConfig.TagClasses[3];
                        break;
                    case TopicCategory.Share:
                        tagClass = ConstConfig.TagClasses[1];
                        break;
                    case TopicCategory.Discuss:
                        tagClass = ConstConfig.TagClasses[2];
                        break;
                    case TopicCategory.Suggest:
                        tagClass = ConstConfig.TagClasses[2];
                        break;
                    case TopicCategory.Notice:
                        tagClass = ConstConfig.TagClasses[0];
                        break;
                    default:
                        break;
                }
            }
        }

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