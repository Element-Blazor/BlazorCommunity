using Blazui.Community.App.Model;
using Blazui.Community.App.Pages;
using Blazui.Community.Model.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
namespace Blazui.Community.App.Shared
{
    public class AppHeaderBase : PageBase
    {
        internal List<TopNaviHeaderMenuModel> headerMenus = new List<TopNaviHeaderMenuModel>();

        [Inject]
        private IOptionsMonitor<List<TopNaviHeaderMenuModel>> Options { get; set; }

        internal BZUserModel CurrentUser;

        protected override async Task InitilizePageDataAsync()
        {
            headerMenus = Options.CurrentValue;
            Options.OnChange(menus => headerMenus = Options.CurrentValue);
            CurrentUser = await GetUser();
        }

        protected void Personal()
        => NavigationManager.NavigateTo("/user/base", forceLoad: true);

        protected void Login()
        => NavigationManager.NavigateTo("/account/signin?returnUrl=" + WebUtility.UrlEncode(new Uri(NavigationManager.Uri).PathAndQuery));
        protected void Regist() => NavigationManager.NavigateTo("/account/register", true);
        protected void LoginOut()
        => NavigationManager.NavigateTo("/account/signout2?returnUrl=" + WebUtility.UrlEncode(new Uri(NavigationManager.Uri).PathAndQuery), true);
    }
}