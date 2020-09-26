using Microsoft.AspNetCore.Authorization;

namespace BlazorCommunity.App.Components
{
    [Authorize]
    public class PersonalTopicBase : PersonalPageBase
    {
        protected override void InitTabTitle()
        {
            tabTitle = "我的帖子";
        }
    }
}