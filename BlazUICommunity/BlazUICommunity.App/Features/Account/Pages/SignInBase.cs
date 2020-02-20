using Blazui.Community.App.Pages;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
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
            if (!signInForm.IsValid())
                return;
            var signInModel = signInForm.GetValue<SignInModel>();
            var user = await userManager.FindByNameAsync(signInModel.userAccount);
            if (user.Status != 0)
            {
                ToastError("账号已被封，请联系管理员");
                return;
            }
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
                ToastError("登录失败，用户名或密码错误");
                return;
            }

        }

        protected void Regist()
        {
            navigationManager.NavigateTo("/account/register" , forceLoad: true);
        }

        protected void FindPwd()
        {
            ToastInfo("尚未实现");
        }

        protected void SSOWX()
        {
            ToastInfo("尚未实现");
        }
        protected void SSOQQ()
        {
            ToastInfo("尚未实现");
        }
        protected void SSOGithub()
        {
            ToastInfo("尚未实现");
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
