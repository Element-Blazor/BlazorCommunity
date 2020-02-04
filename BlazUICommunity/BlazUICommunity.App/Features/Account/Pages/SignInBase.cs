using Blazui.Community.App.Pages;
using Blazui.Community.App.Shared;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using Blazui.Component;
using Blazui.Component.Form;
using Blazui.Component.Popup;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Features.Account.Pages
{
    public partial class SignInBase : PageBase
    {

        [Inject]
        IDataProtectionProvider dataProtectionProvider { get; set; }
        [Parameter]
        public SignInModel signInModel { get; set; }

        public  BForm signInForm;
        protected async Task Login()
        {
            var signInModel = signInForm.GetValue<SignInModel>();
            var user = await userManager.FindByNameAsync(signInModel.userAccount);

            if ( user != null && await userManager.CheckPasswordAsync(user , signInModel.Password) )
            {
                var token = await userManager.GenerateUserTokenAsync(user , TokenOptions.DefaultProvider , "SignIn");

                var data = $"{user.Id}|{token}";

                var parsedQuery = System.Web.HttpUtility.ParseQueryString(new Uri(navigationManager.Uri).Query);

                var returnUrl = parsedQuery["returnUrl"];

                if ( !string.IsNullOrWhiteSpace(returnUrl) )
                {
                    data += $"|{returnUrl}";
                }
               
                var protector = dataProtectionProvider.CreateProtector("SignIn");
                var pdata = protector.Protect(data);
                navigationManager.NavigateTo("/account/signinactual?t=" + pdata , forceLoad: true);
            }
            else
            {
                MessageService.Show("登录失败，用户名或密码错误" , MessageType.Error);
                return;
            }

        }

        protected void Regist()
        {
            navigationManager.NavigateTo("/account/register" , forceLoad: true);
        }

        protected void FindPwd()
        {
            navigationManager.NavigateTo("/account/register" , forceLoad: true);
        }

        protected async Task SSOWX()
        {
            Console.WriteLine("SSOWX");
        }
        protected async Task SSOQQ()
        {
            Console.WriteLine("SSOQQ");
        }
        protected async Task SSOGithub()
        {
            Console.WriteLine("SSOGithub");
        }

        protected override Task InitilizePageDataAsync()
        {
            return Task.CompletedTask;
        }
    }
    public class SignInModel
    {
        [Required]
        public string userAccount { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
