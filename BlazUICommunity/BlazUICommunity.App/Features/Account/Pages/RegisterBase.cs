using Blazui.Community.App.Pages;
using Blazui.Community.DTO;
using Blazui.Community.Repository;
using Blazui.Component;
using Blazui.Component.Input;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Blazui.Community.App.Features.Account.Pages
{
    public class RegisterBase : PageBase
    {
        [Inject]
        private BZUserIdentityRepository BZUserRepository { get; set; }

        [Inject]
        private IHttpContextAccessor httpContextAccessor { get; set; }

        [Inject]
        private IDataProtectionProvider dataProtectionProvider { get; set; }

        protected BForm registerForm;
        internal RegisterAccountDto Value;
        private readonly string CheckChinaPattern = @"[\u4e00-\u9fa5]";//检查汉字的正则表达式

        protected InputType passwordType { get; set; } = InputType.Password;

        internal void TogglePassword() => passwordType = passwordType == InputType.Text ? InputType.Password : InputType.Text;

        protected InputType ConfirmPasswordType { get; set; } = InputType.Password;

        internal void ToggleConfirmPassword() => ConfirmPasswordType = ConfirmPasswordType == InputType.Text ? InputType.Password : InputType.Text;

        protected async Task RegisterUser()
        {
            if (!registerForm.IsValid())
            {
                return;
            }
            var registerAccountModel = registerForm.GetValue<RegisterAccountDto>();
            if (ContainsChineseCharacters(registerAccountModel.UserAccount))
            {
                ToastError("不支持中文账号");
                return;
            }

            if (!registerAccountModel.Password.Equals(registerAccountModel.ConfirmPassword))
            {
                ToastError("两次密码输入不一致");
                return;
            }
            var user = await userManager.FindByNameAsync(registerAccountModel.UserAccount);
            if (user != null)
            {
                ToastError("用户账号已存在");
                return;
            }
            var identityResult = await BZUserRepository.CreateUserAsync(registerAccountModel.UserAccount, registerAccountModel.Password);
            if (!identityResult.Succeeded)
            {
                foreach (var identityError in identityResult.Errors)
                {
                    ToastError(identityError.Description);
                }
                return;
            }
            else
            {
                ToastSuccess("注册成功，正在自动登陆...");
                await Task.Delay(1000);

                await WithFullScreenLoading(async () =>
                {
                    await AutoLogin(registerAccountModel.UserAccount);
                });
            }
        }

        private bool ContainsChineseCharacters(string input)
        {
            return Regex.Matches(input, CheckChinaPattern)?.Count > 0;
        }

        private async Task AutoLogin(string UserAccount)
        {
            var user = await userManager.FindByNameAsync(UserAccount);
            var token = await userManager.GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, "SignIn");
            var data = $"{user.Id}|{token}";

            var parsedQuery = System.Web.HttpUtility.ParseQueryString(new Uri(navigationManager.Uri).Query);

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
            navigationManager.NavigateTo("/account/signinactual?t=" + pdata, forceLoad: true);
        }

        protected override void OnParametersSet()
        {
            Value = new RegisterAccountDto();
            base.OnParametersSet();
        }

        protected override Task InitilizePageDataAsync()
        {
            return Task.CompletedTask;
        }
    }
}