using Blazui.Community.App.Components;
using Blazui.Community.DTO;
using Blazui.Component.Container;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace Blazui.Community.App.Pages
{
    public class ReplyTopicBase : PageBase
    {
        protected int TopicId;
        protected BLayout bLayout;
        protected BTab bTab;
        protected BCard bCard;
        protected TopicContent TopicContent;
        protected ReplyList replyList;
        protected List<BZReplyDtoWithUser> Replys { get; set; }
        protected override async Task InitilizePageDataAsync()
        {
            var parsedQuery = HttpUtility.ParseQueryString(new Uri(navigationManager.Uri).Query);
            int.TryParse(parsedQuery["tpid"], out TopicId);

            if (TopicId < 0)
            {
                MessageService.Show("topicid error", Component.MessageType.Warning);
                return;
            }
            await Task.CompletedTask;
        }


        protected override bool ShouldRender()
        {
            return true;
        }
        internal void NewTopic()
        {
            navigationManager.NavigateTo("new");
        }
        internal void GoHome()
        {
            navigationManager.NavigateTo("/");
        }
    }

}
