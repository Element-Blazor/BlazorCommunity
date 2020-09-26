using Microsoft.AspNetCore.Authorization;

namespace BlazorCommunity.WasmApp.Components.User
{
    [Authorize]
    public partial class PersonalPassword : PersonalPageBase
    {
        protected override bool ShouldRender() => true;

        protected override void InitTabTitle() => tabTitle = "修改密码";
    }
}