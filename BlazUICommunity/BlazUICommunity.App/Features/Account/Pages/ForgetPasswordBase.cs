﻿using Blazui.Community.App.Model;
using Blazui.Community.App.Pages;
using Blazui.Community.Enums;
using Blazui.Component;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Blazui.Community.App.Features.Account.Pages
{
    public class ForgetPasswordBase : PageBase
    {
        protected BForm form;
        protected bool btnSendIsDisabled = false;

        internal int TimeOut { get; set; } = CountDownTime;
        internal string BtnText = "发送验证码";
        protected string PreviousRoute = "/account/signin";

        internal async Task CheckAndNavigatToReset()
        {
            if (string.IsNullOrWhiteSpace(verifyCode))
            {
                ToastError("请先发送验证码到您的邮箱");
                return;
            }
            if (!form.IsValid())
                return;
            var model = form.GetValue<PasswordModel>();
            if (!model.Code.Equals(verifyCode))
            {
                ToastError("验证码无效");
                return;
            }
            else
            {
                var response = await NetService.ValidateVerifyCode(UserId, VerifyCodeType.EmailRetrievePassword, model.Code);
                if (response.IsSuccess)
                    navigationManager.NavigateTo($"/account/reset/{UserId}", true);
                else
                    ToastError(response.Message);
            }
        }

        private string verifyCode = string.Empty;
        private string UserId = string.Empty;

        internal async Task SendEmail()
        {
            if (!form.IsValid())
                return;

            var model = form.GetValue<PasswordModel>();
            if (!Regex.Match(model.Email, "^[a-z0-9]+([._\\-]*[a-z0-9])*@([a-z0-9]+[-a-z0-9]*[a-z0-9]+.){1,63}[a-z0-9]+$").Success)
            {
                ToastError("邮箱号码错误");
                return;
            }
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                ToastError("该邮箱尚未绑定过任何账号");
            }
            else
            {
                UserId = user.Id;
                await WithFullScreenLoading(
                    async () => await NetService.SendVerifyCode(user.Id, VerifyCodeType.EmailRetrievePassword, model.Email),
                    response =>
                    {
                        if (response.IsSuccess)
                        {
                            verifyCode = response.Data.ToString();
                            ToastSuccess("发送成功，请到邮箱查收,如果未收到，可以查看一下垃圾邮箱");
                            btnSendIsDisabled = true;
                            StateHasChanged();
                        }
                        else
                            ToastError($"发送失败，{response.Message}");
                    }

            );
                while (TimeOut > 0)
                {
                    if (TimeOut == 1) btnSendIsDisabled = false;
                    TimeOut--;
                    BtnText = (TimeOut > 0 && TimeOut < CountDownTime) ? $"请等待{TimeOut}s后重试" : "发送验证码";
                    StateHasChanged();
                    await Task.Delay(1000);
                };
                TimeOut = CountDownTime;
                StateHasChanged();
            }
        }
    }
}