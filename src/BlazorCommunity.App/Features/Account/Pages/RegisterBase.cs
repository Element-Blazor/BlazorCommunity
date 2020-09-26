using BlazorCommunity.App.Pages;
using BlazorCommunity.Common;
using BlazorCommunity.DTO;
using BlazorCommunity.Model.Models;
using Element;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlazorCommunity.App.Features.Account.Pages
{
    public class RegisterBase : PageBase
    {
        [Inject]
        UserManager<BZUserModel> userManager { get; set; }

        [Inject]
        private IHttpContextAccessor httpContextAccessor { get; set; }

        [Inject]
        private IDataProtectionProvider dataProtectionProvider { get; set; }

        protected BForm registerForm;
        internal RegisterAccountDto Value = new RegisterAccountDto();

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
            await WithFullScreenLoading(async () =>
            {
                var registerAccountModel = registerForm.GetValue<RegisterAccountDto>();
                if (RegexHelper.ContainsChineseCharacters(registerAccountModel.UserAccount))
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

                bool AccountIsEmail =!string.IsNullOrWhiteSpace(registerAccountModel.Email)&& RegexHelper.IsEmail(registerAccountModel.Email);

                var identityResult = await CreateUserAsync(
                    registerAccountModel.UserAccount,
                    registerAccountModel.Password,
                    AccountIsEmail ? registerAccountModel.Email : null,
                    registerAccountModel.QQ);

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
                    ToastSuccess("注册成功，即将自动登录...");
                    await Task.Delay(1000);
                    await AutoLogin(registerAccountModel.UserAccount);
                }
            });
        }



        private async Task AutoLogin(string UserAccount)
        {
            var user = await userManager.FindByNameAsync(UserAccount);
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



        public async Task<IdentityResult> CreateUserAsync(string userAccount, string Password, string Email = null, string QQ = null)
        {
            if (string.IsNullOrEmpty(userAccount))
            {
                throw new ArgumentException("message", nameof(userAccount));
            }

            if (string.IsNullOrEmpty(Password))
            {
                throw new ArgumentException("message", nameof(Password));
            }
            return await userManager.CreateAsync(
                   new BZUserModel
                   {
                       UserName = userAccount,
                       NickName = userAccount,
                       Email = Email ?? "",
                       EmailConfirmed = false,
                       NormalizedUserName = userAccount,
                       CreateDate = DateTime.Now,
                       LastLoginDate = DateTime.Now,
                       CreatorId = Guid.Empty.ToString(),
                       Sex = 0,
                       Status = 0,
                       Avator = "",
                       PhoneNumber = "",
                       QQ = QQ ?? ""
                   }, Password);
        }
    }
}