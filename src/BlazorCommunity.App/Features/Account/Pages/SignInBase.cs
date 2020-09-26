using BlazorCommunity.App.Pages;
using Element;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace BlazorCommunity.App.Features.Account.Pages
{
    public partial class SignInBase : PageBase
    {
        [Inject]
        private IHttpContextAccessor httpContextAccessor { get; set; }

        [Inject]
        private IDataProtectionProvider dataProtectionProvider { get; set; }

        protected InputType passwordType { get; set; } = InputType.Password;

        [Parameter]
        public SignInModel signInModel { get; set; }

        public BForm signInForm;

        protected async Task Login()
        {
            if (!signInForm.IsValid())
                return;
            await WithFullScreenLoading(async () =>
            {
                var signInModel = signInForm.GetValue<SignInModel>();
                var user = await userManager.FindByNameAsync(signInModel.UserAccount);
                if (user == null)
                {
                    user = await userManager.FindByEmailAsync(signInModel.UserAccount);
                    if(user==null)
                    {
                        ToastError("账号不存在，请先注册");
                        return;
                    }
                }
                if (user.Status != 0)
                {
                    ToastError("账号已被封，请联系管理员");
                    return;
                }
                if (user != null && await userManager.CheckPasswordAsync(user, signInModel.Password))
                {
                    memoryCache.Remove(user.UserName);    //清除token

                    var token = await userManager.GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, "SignIn");

                    var data = $"{user.Id}|{token}";

                    var parsedQuery = System.Web.HttpUtility.ParseQueryString(new Uri(NavigationManager.Uri).Query);

                    var returnUrl = parsedQuery["returnUrl"];

                    if (!string.IsNullOrWhiteSpace(returnUrl))
                    {
                        data += $"|{returnUrl}";
                    }
                    user.LastLoginDate = DateTime.Now;
                    user.LastLoginType = 0;
                    user.LastLoginAddr = this.httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                    await userManager.UpdateAsync(user);
                    var protector = dataProtectionProvider.CreateProtector("SignIn");
                    var pdata = protector.Protect(data);
                    NavigationManager.NavigateTo("/account/signinactual?t=" + pdata, forceLoad: true);
                }
                else
                {
                    ToastError("登录失败，用户名或密码错误");
                    return;
                }
            });
        }

        internal void TogglePassword() => passwordType = passwordType == InputType.Text ? InputType.Password : InputType.Text;

        protected void Regist() => NavigationManager.NavigateTo("/account/register", true);

        protected void ForgetPwd() => NavigationManager.NavigateTo("/account/forget", true);

      

        protected override Task InitilizePageDataAsync()
        {
            return Task.CompletedTask;
        }
    }

    public class SignInModel
    {
        [Required]
        public string UserAccount { get; set; }

        [Required]
        public string Password { get; set; }
    }
}