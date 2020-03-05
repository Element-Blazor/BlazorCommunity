using Blazui.Community.App.Pages;
using Blazui.Community.Model.Models;
using System;
using System.Threading.Tasks;

namespace Blazui.Community.App.Shared
{
    public class AppHeaderBase : PageBase
    {

        internal BZUserModel CurrentUser;
        protected override async Task InitilizePageDataAsync()
        {
            CurrentUser = await GetUser();
        }
      
        protected void NavigateToLogin()
        {
            navigationManager.NavigateTo("/account/signin?returnUrl=" + System.Net.WebUtility.UrlEncode(new Uri(navigationManager.Uri).PathAndQuery));
        }

        protected void NavigateToRoot()
        {
            navigationManager.NavigateTo("/" , forceLoad: true);
        }
        protected void NavigateToComponent()
        {
            navigationManager.NavigateTo(Constant.ComponentUrl, forceLoad: true);
        }
        protected void NavigateToDemo()
        {
            navigationManager.NavigateTo(Constant.DemoUrl, forceLoad: true);
        }
        
        protected void NavigateToDoc()
        {
            navigationManager.NavigateTo(Constant.DocsUrl, forceLoad: true);
        }
        protected void GotoUCentor()
        {
            navigationManager.NavigateTo("/user/base" , forceLoad: true);
        }
        protected void LoginOut()
        {
            navigationManager.NavigateTo("/account/signout2?returnUrl=" + System.Net.WebUtility.UrlEncode(new Uri(navigationManager.Uri).PathAndQuery),true);
        }
    }
}
