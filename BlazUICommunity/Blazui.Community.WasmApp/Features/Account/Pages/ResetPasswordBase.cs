using Blazui.Community.WasmApp.Model;
using Blazui.Community.WasmApp.Pages;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Blazui.Community.WasmApp.Features.Account.Pages
{
    public class ResetPasswordBase : PageBase
    {
        protected string PreviousRoute = "/account/forget";
        protected BForm form;

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
                var user = await NetService.FindUserByIdAsync(UserId);
                if (user is null)
                {
                    ToastError("用户不存在");
                    return;
                }

                var resetResult = await NetService.ResetPasswordAsync(new DTO.App.UpdateUserPasswordDto { NewPassword = model.Password, UserId = UserId }) ;
                if (resetResult.IsSuccess)
                {
                    ToastSuccess("密码重置成功，请稍后使用新密码登录");
                    await WithFullScreenLoading(async () => await Task.Delay(1500));
                    NavigationManager.NavigateTo("/account/signin", true);
                }
                else
                {
                    ToastError(resetResult.Message);
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