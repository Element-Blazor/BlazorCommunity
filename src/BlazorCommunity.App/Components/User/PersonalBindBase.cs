using Microsoft.AspNetCore.Authorization;

namespace BlazorCommunity.App.Components
{
    [Authorize]
    public class PersonalBindBase : PersonalPageBase
    {
        protected override void InitTabTitle()
        {
            tabTitle = "我的绑定";
        }
    }
}