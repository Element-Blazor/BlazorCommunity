using Blazui.Community.WasmApp.Model;
using Blazui.Community.WasmApp.Pages;
using Blazui.Community.Enums;
using Blazui.Community.Model.Models;
using Blazui.Component;
using System.Threading.Tasks;
using Blazui.Community.DTO.App;
using Blazui.Community.DTO;

namespace Blazui.Community.WasmApp.Components.User
{
    public class ChangePasswordByEmailBase : PageBase
    {
        protected BForm form;
        protected BForm resetPasswordForm;
        protected bool btnInputDisabled = false;

        internal int TimeOut { get; set; } = CountDownTime;
        internal string BtnText = "发送验证码";
        private string verifyCode = string.Empty;

        protected bool CheckverifyCodeSuccess = false;

        internal async Task SendEmail()
        {
            if (!form.IsValid())
                return;
            var model = form.GetValue<PasswordModel>();
            if (!RegexHelper.IsEmail(model.Email))
            {
                ToastError("邮箱号码错误");
                return;
            }
            User = await GetUser();
            if (User is null)
            {
                ToastError("您尚未绑定邮箱");
            }
            else if (!User.Email.Equals(model.Email))
            {
                ToastError("请输入您已绑定的邮箱");
            }
            else
            {
                await WithFullScreenLoading(
                    async () => await NetService.SendVerifyCode(User.Id, EmailType.EmailRetrievePassword, model.Email),
                   async response =>
                    {
                        if (response.IsSuccess)
                        {
                            verifyCode = response.Data.ToString();
                            ToastSuccess("验证码发送成功，2分钟内有效，请前往邮箱查收");
                            btnInputDisabled = true;
                            while (TimeOut > 0)
                            {
                                if (TimeOut == 1) btnInputDisabled = false;
                                TimeOut--;
                                BtnText = (TimeOut > 0 && TimeOut < CountDownTime) ? $"请等待{TimeOut}s后重试" : "发送验证码";
                                StateHasChanged();
                            };
                            TimeOut = CountDownTime;
                            await Task.Delay(1000);
                            StateHasChanged();
                        }
                        else
                            ToastError($"{response.Message}");
                    }

            );
            }
        }

        internal async Task CheckAndNavigatToReset()
        {
            var model = form.GetValue<PasswordModel>();
            if (model.Code != verifyCode)
            {
                ToastError("验证码无效");
                return;
            }
            else
            {
                var response = await NetService.ValidateVerifyCode(User.Id, EmailType.EmailRetrievePassword, model.Code);
                if (response.IsSuccess)
                {
                    CheckverifyCodeSuccess = true;
                    StateHasChanged();
                }
                else
                    ToastError(response.Message);
            }
        }

        internal async Task ReSetPassword()
        {
            if (!resetPasswordForm.IsValid()) return;

            var model = resetPasswordForm.GetValue<PasswordModel>();
            if (model.Password.Equals(model.ConfirmPassword))
            {
                var resetResult = await NetService.ResetPasswordAsync(new UpdateUserPasswordDto()
                {
                    UserId = User.Id,
                    NewPassword = model.Password
                }); ;
                if (resetResult.IsSuccess)
                {
                    ToastSuccess("密码修改成功，下次登录请使用新密码");
                    resetPasswordForm.Reset();
                    form.Reset();
                    CheckverifyCodeSuccess = false;
                    StateHasChanged();
                }
                else
                {
                    ToastError(resetResult.Message);
                }
            }
            else
            {
                ToastError("两次密码输入不一致");
            }
        }
    }
}