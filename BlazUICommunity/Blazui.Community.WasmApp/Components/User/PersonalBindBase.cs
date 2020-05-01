using Microsoft.AspNetCore.Authorization;

namespace Blazui.Community.WasmApp.Components
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