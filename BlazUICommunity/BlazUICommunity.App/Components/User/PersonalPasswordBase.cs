using Microsoft.AspNetCore.Authorization;

namespace Blazui.Community.App.Components
{
    [Authorize]
    public class PersonalPasswordBase : PersonalPageBase
    {
        protected override bool ShouldRender() => true;

        protected override void InitTabTitle() => tabTitle = "修改密码";
    }
}