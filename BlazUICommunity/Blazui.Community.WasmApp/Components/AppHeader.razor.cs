using Blazui.Community.WasmApp.Model;
using Blazui.Community.WasmApp.Pages;
using Blazui.Community.WasmApp.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Blazui.Community.WasmApp.Components
{
    public partial class AppHeader : PageBase
    {
        protected List<TopNaviHeaderMenuModel> headerMenus = new List<TopNaviHeaderMenuModel>();
        [Inject]
        IOptionsMonitor<TopNavMenuOption> Options { get; set; }

        protected override async Task InitilizePageDataAsync()
        {
            User = await GetUser();
            if (User != null && string.IsNullOrWhiteSpace(User.Avator))
                User.Avator = "/img/defaultact.png";
            headerMenus = Options.CurrentValue.HeaderMenus;
            Options.OnChange(menus => headerMenus = Options.CurrentValue.HeaderMenus);
        }

        protected void Personal() => NavigationManager.NavigateTo("/user/base", forceLoad: true);

        protected void Login()
        => NavigationManager.NavigateTo("/account/signin?returnUrl=" + WebUtility.UrlEncode(new Uri(NavigationManager.Uri).PathAndQuery));
        protected void Regist() => NavigationManager.NavigateTo("/account/register", true);
        protected void LoginOut()
        => NavigationManager.NavigateTo("/account/signout2?returnUrl=" + WebUtility.UrlEncode(new Uri(NavigationManager.Uri).PathAndQuery), true);


        internal string SearchText { get; set; }
        internal string SpecialCharacter = "@_~$%^*+#￥&=";

        internal void Search()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
                return;
            SearchText = RemoveSpecialCharacter(SearchText);
            NavigationManager.NavigateTo($"/m/search/{SearchText}", true);
        }

        /// <summary>
        /// 去掉特殊字符
        /// </summary>
        /// <param name="hexData"></param>
        /// <returns></returns>
        string RemoveSpecialCharacter(string hexData)
        {
            return Regex.Replace(hexData, "[ \\[ \\] \\^ \\-_*×――(^)|'$%~!@#$…&%￥—+=<>《》!！??？:：•`·、。，；,.;\"‘’“”-]", "");
        }
    }
}