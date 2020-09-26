using Microsoft.AspNetCore.Authorization;

namespace BlazorCommunity.App.Components
{
    [Authorize]
    public class PersonalPasswordBase : PersonalPageBase
    {
        protected override bool ShouldRender() => true;

        protected override void InitTabTitle() => tabTitle = "修改密码";
    }
}