using Blazui.Community.WasmApp.Model;
using Blazui.Community.WasmApp.Pages;
using Blazui.Community.WasmApp.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Blazui.Community.WasmApp.Shared
{
    public class AppHeaderBase : PageBase
    {
        internal List<TopNaviHeaderMenuModel> headerMenus = new List<TopNaviHeaderMenuModel>();

        [Inject]
        private IOptionsMonitor<List<TopNaviHeaderMenuModel>> Options { get; set; }

        [Inject]
        private IAuthenticationService authenticationService { get; set; }

        protected override async Task InitilizePageDataAsync()
        {
            headerMenus = Options.CurrentValue;
            Options.OnChange(menus => headerMenus = Options.CurrentValue);
            User = await GetUser();
        }

        protected void Personal()
        => NavigationManager.NavigateTo("/user/base", forceLoad: true);

        protected void Login()
        => NavigationManager.NavigateTo("/account/signin?returnUrl=" + WebUtility.UrlEncode(new Uri(NavigationManager.Uri).PathAndQuery));
        protected void Regist() => NavigationManager.NavigateTo("/account/register?returnUrl=" + WebUtility.UrlEncode(new Uri(NavigationManager.Uri).PathAndQuery), true);
        protected async Task LoginOut()
        {
            await authenticationService.Logout();
            await NavigateToReturnUrl();
        }
    }
}