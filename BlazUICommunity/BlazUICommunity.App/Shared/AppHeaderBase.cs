using Blazui.Community.App.Pages;
using Blazui.Component.NavMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Shared
{
    public class AppHeaderBase : PageBase
    {

        protected bool IsLogin = false;
        protected override async Task InitilizePageDataAsync()
        {

            var userstatue = await authenticationStateTask;
            IsLogin = userstatue.User.Identity.IsAuthenticated;
            BzUser = await userManager.GetUserAsync(userstatue.User);
        }
      
        protected void NavigateToLogin()
        {
            navigationManager.NavigateTo("/account/signin" , forceLoad: true);
        }

        protected void NavigateToRoot()
        {
            navigationManager.NavigateTo("/" , forceLoad: true);
        }
        protected void GotoUCentor()
        {
            navigationManager.NavigateTo("/user" , forceLoad: true);
        }
        protected void LoginOut()
        {
            navigationManager.NavigateTo("/account/signout" , forceLoad: true);
        }
    }
}
