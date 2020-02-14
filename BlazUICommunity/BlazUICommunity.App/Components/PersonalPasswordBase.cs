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

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
                UpdateUI();
        }

        private void UpdateUI()
        {
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
            {
                return;
            }
            var activity = checkChangePwdForm.GetValue<PasswordModel>();
            if (VerifyCode != activity.VerifyCode)
                return;
            var result = await NetService.VerifyVerifyCode(User.Id, 1, VerifyCode);
            if (result.IsSuccess)
            {
                IsCheckBindMobileSuccess = true;
            }
        }



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

        protected async Task ChangePwd()
        {
            if (!changePwdForm.IsValid())
                return;
            var activity = changePwdForm.GetValue<PasswordModel>();
            if (activity.Password == activity.ConfirmPassword)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(User);
                var result = await userManager.ResetPasswordAsync(User, token, activity.Password);
                if (result.Succeeded)
                {
                    MessageService.Show("修改成功,需要重新登录", MessageType.Success);
                    await Task.Delay(1000);
                    navigationManager.NavigateTo("/account/signout2", forceLoad: true);
                }
            }
        }

        protected override void InitTabTitle()
        {
            tabTitle = "我的密码";
        }
    }
}
