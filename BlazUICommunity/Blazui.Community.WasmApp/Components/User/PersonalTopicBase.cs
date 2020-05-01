using Microsoft.AspNetCore.Authorization;

namespace Blazui.Community.WasmApp.Components
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