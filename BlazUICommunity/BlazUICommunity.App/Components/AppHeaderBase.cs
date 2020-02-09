using Blazui.Community.App.Pages;
using Blazui.Community.Model.Models;
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
        protected BZUserModel BzUser { get;private set; }
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
            navigationManager.NavigateTo("/user/base" , forceLoad: true);
        }
        protected void LoginOut()
        {
            navigationManager.NavigateTo("/account/signout" , forceLoad: true);
        }
    }
}
