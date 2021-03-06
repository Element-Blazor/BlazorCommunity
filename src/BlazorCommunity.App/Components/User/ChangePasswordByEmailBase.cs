﻿using BlazorCommunity.App.Model;
using BlazorCommunity.App.Pages;
using BlazorCommunity.Common;
using BlazorCommunity.Enums;
using BlazorCommunity.Model.Models;
using Element;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlazorCommunity.App.Components.User
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