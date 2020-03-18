using Blazui.Community.App.Model;
using Blazui.Community.App.Model.ViewModel;
using Blazui.Community.App.Pages;
using Blazui.Community.Enums;
using Blazui.Community.Model.Models;
using Blazui.Component;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Blazui.Community.App.ConstantConfiguration;

namespace Blazui.Community.App.Components.User
{
    public class ChangePasswordByEmailBase : PageBase
    {
        protected BForm form;
        protected BForm resetPasswordForm;
        protected bool btnInputDisabled = false;

        internal int TimeOut { get; set; } = CountDownTime;
        internal string BtnText = "发送验证码";
        private BZUserModel User;
        private string verifyCode = string.Empty;

        protected bool NewPasswordFormShow = false;

        internal async Task SendEmail()
        {
            if (!form.IsValid())
                return;
            var model = form.GetValue<ForgetPasswordModel>();
            if (!Regex.Match(model.Email, "^[a-z0-9]+([._\\-]*[a-z0-9])*@([a-z0-9]+[-a-z0-9]*[a-z0-9]+.){1,63}[a-z0-9]+$").Success)
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
                    async () => await NetService.SendVerifyCode(User.Id, VerifyCodeType.EmailRetrievePassword, model.Email),
                    response =>
                    {
                        if (response.IsSuccess)
                        {
                            verifyCode = response.Data.ToString();
                            ToastSuccess("发送成功，请到邮箱查收,如果未收到，可以查看一下垃圾邮箱");
                            btnInputDisabled = true;
                            StateHasChanged();
                        }
                        else
                            ToastError($"发送失败，{response.Message}");
                    }

            );
                while (TimeOut > 0)
                {
                    if (TimeOut == 1) btnInputDisabled = false;
                    TimeOut--;
                    BtnText = (TimeOut > 0 && TimeOut < CountDownTime) ? $"请等待{TimeOut}s后重试" : "发送验证码";
                    StateHasChanged();
                    await Task.Delay(1000);
                };
                TimeOut = CountDownTime;
                StateHasChanged();
            }
        }

        internal async Task CheckAndNavigatToReset()
        {
            var model = form.GetValue<ForgetPasswordModel>();
            if (model.VerCode != verifyCode)
            {
                ToastError("验证码无效");
                return;
            }
            else
            {
                var response = await NetService.ValidateVerifyCode(User.Id, VerifyCodeType.EmailRetrievePassword, model.VerCode);
                if (response.IsSuccess)
                {
                    NewPasswordFormShow = true;
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
                var token = await userManager.GeneratePasswordResetTokenAsync(User);
                var resetResult = await userManager.ResetPasswordAsync(User, token, model.Password);
                if (resetResult.Succeeded)
                {
                    ToastSuccess("密码修改成功，下次登录请使用新密码");
                    resetPasswordForm.Reset();
                    form.Reset();
                    NewPasswordFormShow = false;
                    StateHasChanged();
                }
                else
                {
                    foreach (var item in resetResult.Errors)
                    {
                        ToastError(item.Description);
                    }
                }
            }
            else
            {
                ToastError("两次密码输入不一致");
            }
        }
    }
}