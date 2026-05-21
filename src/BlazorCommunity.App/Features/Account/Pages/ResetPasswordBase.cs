using BlazorCommunity.App.Model;
using BlazorCommunity.App.Pages;
using Element;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace BlazorCommunity.App.Features.Account.Pages
{
    public class ResetPasswordBase : PageBase
    {
        protected string PreviousRoute = "/account/forget";
        protected ElForm form;

        [Parameter]
        public string UserId { get; set; }

        internal async Task ResetPassword()
        {
            if (string.IsNullOrWhiteSpace(UserId))
                return;
            if (!Guid.TryParse(UserId, out Guid guid))
                NavigationManager.NavigateTo(PreviousRoute, true);
            if (!form.IsValid())
                return;
            var model = form.GetValue<PasswordModel>();
            if (model.ConfirmPassword.Equals(model.Password))
            {
                var user = await userManager.FindByIdAsync(UserId);
                if (user is null)
                {
                    ToastError("该邮箱未绑定用户");
                    return;
                }
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                if (token is null)
                {
                    ToastError("获取token失败");
                    return;
                }
                var resetResult = await userManager.ResetPasswordAsync(user, token, model.Password);
                if (resetResult.Succeeded)
                {
                    ToastSuccess("密码重置成功，请稍后使用新密码登录");
                    await WithFullScreenLoading(async () => await Task.Delay(1500));
                    NavigationManager.NavigateTo("/account/signin", true);
                }
                else
                {
                    ToastError(JsonConvert.SerializeObject(resetResult.Errors));
                    return;
                }
            }
            else
            {
                ToastError("两次输入密码不一致");
                return;
            }
        }
    }
}