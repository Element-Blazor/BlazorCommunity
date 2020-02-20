using Blazui.Community.App.Data;
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

            BzUser = await GetUser();
            IsLogin = BzUser != null;
           
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
            navigationManager.NavigateTo(ConstantUtil.ComponentUrl, forceLoad: true);
        }
        protected void NavigateToDemo()
        {
            navigationManager.NavigateTo(ConstantUtil.DemoUrl, forceLoad: true);
        }
        
        protected void NavigateToDoc()
        {
            navigationManager.NavigateTo(ConstantUtil.DocsUrl, forceLoad: true);
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
