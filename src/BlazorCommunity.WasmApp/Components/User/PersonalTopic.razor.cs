using Microsoft.AspNetCore.Authorization;

namespace BlazorCommunity.WasmApp.Components.User
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