using Microsoft.AspNetCore.Authorization;

namespace Blazui.Community.WasmApp.Components.User
{
    [Authorize]
    public partial class PersonalPassword : PersonalPageBase
    {
        protected override bool ShouldRender() => true;

        protected override void InitTabTitle() => tabTitle = "修改密码";
    }
}