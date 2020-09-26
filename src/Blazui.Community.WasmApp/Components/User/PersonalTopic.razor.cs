using Microsoft.AspNetCore.Authorization;

namespace Blazui.Community.WasmApp.Components.User
{
    [Authorize]
    public partial class PersonalTopic : PersonalPageBase
    {
        protected override void InitTabTitle()
        {
            tabTitle = "我的帖子";
        }
    }
}