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
        => navigationManager.NavigateTo("/user/base", forceLoad: true);

        protected void Login()
        => navigationManager.NavigateTo("/account/signin?returnUrl=" + WebUtility.UrlEncode(new Uri(navigationManager.Uri).PathAndQuery));

        protected void LoginOut()
        => navigationManager.NavigateTo("/account/signout2?returnUrl=" + WebUtility.UrlEncode(new Uri(navigationManager.Uri).PathAndQuery), true);
    }
}