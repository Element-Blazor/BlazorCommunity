using Blazui.Community.App.Model;
using Blazui.Community.App.Pages;
using Blazui.Community.Model.Models;
using Blazui.Component;
using Blazui.Component.Button;
using Blazui.Component.Container;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using static Blazui.Community.App.Data.ConstantUtil;

namespace Blazui.Community.App.Components
{
    [Authorize]
    public class PersonalPasswordBase : PersonalPageBase
    {

        protected BTabPanel BTabPanel { get; set; }
        protected BForm checkChangePwdForm { get; set; }
        protected BForm changePwdForm { get; set; }
        protected BTab Btab { get; set; }
        internal PasswordModel valueCheckChangePwd { get; set; }
        internal PasswordModel valueChangePwd { get; set; }
        internal bool IsDisabled { get; set; } = false;
        internal int countdown { get; set; } = CountDownTime;
        protected BButton btnSendmsg { get; set; }
        protected BFormActionItem bFormActionItemMsg { get; set; }

        protected BCard BCard1 { get; set; }
        protected BCard BCard2 { get; set; }

        protected string VerifyCode = "";

        protected BForm bformOldpwd;



        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
                UpdateUI();
        }

        private void UpdateUI()
        {
            bformOldpwd?.Refresh();
            bFormActionItemMsg?.Refresh();
            checkChangePwdForm?.Refresh();
            btnSendmsg?.Refresh();
            BTabPanel?.Refresh();
            Btab?.Refresh();
            BCard1?.Refresh();
        }

        protected override bool ShouldRender()
        {
            return true;
        }


        protected async Task CheckVerifyCode()
        {
            if (!checkChangePwdForm.IsValid())
                return;
            var PasswordModel = checkChangePwdForm.GetValue<PasswordModel>();
            if (VerifyCode != PasswordModel.VerifyCode)
                return;
            var result = await NetService.VerifyVerifyCode(User.Id, 1, VerifyCode);
            IsCheckBindMobileSuccess = result==null?false:result.IsSuccess;
        }


        /// <summary>
        /// 短信验证是否成功
        /// </summary>
        protected bool IsCheckBindMobileSuccess = false;

        protected BZUserModel User;
        protected override async Task InitilizePageDataAsync()
        {
            IsCheckBindMobileSuccess = false;

            User = await GetUser();
            valueCheckChangePwd = new PasswordModel()
            {
                Mobile = User?.PhoneNumber ?? "",
                Email = User?.Email ?? ""
            };
        }

        protected async Task ChangePwdByMobile()
        {
            if (!changePwdForm.IsValid())
                return;
            if (!CheckConfirmPassword())
                return;
            await ResetPassword(changePwdForm);
        }

        protected async Task ChangePwdByOld()
        {
            if (!bformOldpwd.IsValid())
                return;
            if (!await CheckOldPassword())
                return;
            if (!CheckConfirmPassword())
                return;
            await ResetPassword(bformOldpwd);
        }

        private async Task<bool> CheckOldPassword()
        {
            var PasswordModel = bformOldpwd.GetValue<PasswordModel>();
            var checkOld = await userManager.CheckPasswordAsync(User, PasswordModel.OldPassword);
            if (!checkOld)
            {
                ToastError("旧密码错误");
                return false;
            }
            return true;
        }
        private bool CheckConfirmPassword()
        {
            var PasswordModel = bformOldpwd.GetValue<PasswordModel>();
            if (PasswordModel.Password != PasswordModel.ConfirmPassword)
            {
                ToastError("新密码与确认密码不一致");
                return false;
            }
            return true;
        }

        private async Task ResetPassword(BForm bForm)
        {
            var PasswordModel = bForm.GetValue<PasswordModel>();
            var token = await userManager.GeneratePasswordResetTokenAsync(User);
            var result = await userManager.ResetPasswordAsync(User, token, PasswordModel.Password);
            if (result.Succeeded)
            {
                ToastSuccess("密码重置成功,下次登录请使用新密码");
                bForm.Reset();
            }
        }


        protected async Task SendMsg()
        {
            if (!checkChangePwdForm.IsValid())
            {
                return;
            }
            var activity = checkChangePwdForm.GetValue<PasswordModel>();

            var result = await NetService.CreateAndSendVerifyCodeMessage(User.Id, 1, activity.Mobile);
            if (result.IsSuccess)
            {
                VerifyCode = result.Data.ToString();
                IsDisabled = true;
                while (countdown > 0)
                {
                    if (countdown == 1)
                    {
                        IsDisabled = false;
                    }
                    countdown--;
                    UpdateUI();
                    await Task.Delay(1000);
                }
                countdown = CountDownTime;
                UpdateUI();
            }
        }
        protected override void InitTabTitle()
        {
            tabTitle = "我的密码";
        }
    }
}
