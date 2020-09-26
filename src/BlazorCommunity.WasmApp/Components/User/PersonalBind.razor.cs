using Microsoft.AspNetCore.Authorization;

namespace BlazorCommunity.WasmApp.Components.User
{
    [Authorize]
    public partial class PersonalBind : PersonalPageBase
    {
        protected override void InitTabTitle()
        {
            tabTitle = "我的绑定";
        }
    }
}