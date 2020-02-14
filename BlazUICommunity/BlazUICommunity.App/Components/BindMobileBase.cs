using Blazui.Community.App.Data;
using Blazui.Community.App.Model;
using Blazui.Community.App.Pages;
using Blazui.Community.Model.Models;
using Blazui.Component;
using Blazui.Component.Button;
using Blazui.Component.Container;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using static Blazui.Community.App.Data.ConstantUtil;

namespace Blazui.Community.App.Components
{
    [Authorize]
    public class BindMobileBase : PageBase
    {

        protected BForm bForm { get; set; }
        internal BindMobileModel value { get; set; }

        internal bool IsDisabled { get; set; } = false;
        internal int countdown { get; set; } = CountDownTime;
        protected BButton btnSendmsg { get; set; }
        protected BButton btnBindMobile { get; set; }
        protected BFormActionItem ActionMsg { get; set; }
        protected BFormActionItem ActionVerify { get; set; }
        protected bool showMobileInput { get; set; } = false;

        protected BCard bCard { get; set; }
     
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
                UpdateUI();
        }

        private void UpdateUI()
        {
            ActionVerify?.Refresh();
            ActionMsg?.Refresh();
            bForm?.Refresh();
            btnSendmsg?.Refresh();
            bCard?.Refresh();
            btnBindMobile?.Refresh();
        }

        protected override bool ShouldRender()
        {
            return true;
        }

        protected async Task CheckAndBindMobileVerifyCode()
        {
            var activity = bForm.GetValue<BindMobileModel>();
         
            if (VerifyCode != activity.VerifyCode)
                return;
            var result = await NetService.VerifyVerifyCode(User.Id, 2, VerifyCode);
            if (result.IsSuccess)
            {
                var bindMobile = await userManager.SetPhoneNumberAsync(User, activity.Mobile);
                if (bindMobile.Succeeded)
                {
                    MessageService.Show("绑定手机成功", MessageType.Success);
                }
            }


        }

        protected string VerifyCode = "";
        protected async Task SendBindMobileMsg()
        {
            if (!bForm.IsValid())
                return;

            var activity = bForm.GetValue<BindMobileModel>();

            var result = await NetService.CreateAndSendVerifyCodeMessage(User.Id, 2, activity.Mobile);
            if(result.IsSuccess)
            {
                VerifyCode = result.Data.ToString();
                   IsDisabled = true;
                showMobileInput = true;
                Console.WriteLine(result.Data);
                while (countdown > 0)
                {
                    if (countdown == 1)
                    {
                        IsDisabled = false;
                    }
                    countdown--;
                    UpdateUI();
                    await Task.Delay(1000);
                };
                countdown = CountDownTime;
                UpdateUI();
            }
        }
        protected BZUserModel User;
        protected override async Task InitilizePageDataAsync()
        {
        
             User = await GetUser();
            value = new BindMobileModel()
            {
                Mobile = User?.PhoneNumber ?? "",
                VerifyCode = ""
            };
            showMobileInput = !string.IsNullOrWhiteSpace(value.Mobile);
        }
    }
}
