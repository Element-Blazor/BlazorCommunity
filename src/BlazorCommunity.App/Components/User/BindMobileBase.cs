using BlazorCommunity.App.Model;
using BlazorCommunity.App.Pages;
using BlazorCommunity.Enums;
using BlazorCommunity.Model.Models;
using BlazorCommunity.Response;
using Blazui.Component;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlazorCommunity.App.Components
{
    [Authorize]
    public class BindMobileBase : PageBase
    {
        protected BForm bForm { get; set; }
        internal PasswordModel value { get; set; }
        protected bool showInput { get; set; } = false;
        protected BCard bCard { get; set; }
        internal bool IsDisabled { get; set; } = false;
        internal int TimeOut { get; set; } = CountDownTime;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
                UpdateUI();
        }

        private void UpdateUI()
        {
            bForm?.Refresh();
            bCard?.Refresh();
        }

        protected override bool ShouldRender() => true;

        protected string VerifyCode = string.Empty;

        protected async Task SendBindMobileMsg()
        {
            if (!bForm.IsValid())
                return;
            var model = bForm.GetValue<PasswordModel>();
            var mobileValid = Regex.Match(model.Mobile, "^((13[0-9])|(14[5,7])|(15[0-3,5-9])|(17[0,3,5-8])|(18[0-9])|166|198|199|(147))\\d{8}$").Success;
            if (!mobileValid)
            {
                ToastError("手机号码错误");
                return;
            }
            var response = await NetService.SendVerifyCode(User.Id, EmailType.MobileBind, model.Mobile);
            await Watting(response);
        }

        private async Task Watting(BaseResponse response)
        {
            if (response.IsSuccess)
            {
                VerifyCode = response.Data.ToString();
                IsDisabled = true;
                showInput = true;
                ToastSuccess("验证码发送成功，2分钟内有效，请前往邮箱查收");
                while (TimeOut > 0)
                {
                    if (TimeOut == 1)
                    {
                        IsDisabled = false;
                    }
                    TimeOut--;
                    UpdateUI();
                    await Task.Delay(1000);
                };
                TimeOut = CountDownTime;
                UpdateUI();
            }
            else
                ToastError(response.Message);
        }

        protected async Task CheckAndBindMobileVerifyCode()
        {
            if (string.IsNullOrWhiteSpace(VerifyCode))
            {
                ToastError("请先发送验证码到您的邮箱");
                return;
            }
            if (!bForm.IsValid())
                return;

            var model = bForm.GetValue<PasswordModel>();

            if (!model.Code.Equals(VerifyCode))
            {
                ToastError("验证码无效");
                return;
            }
            await SetPhoneNumberAsync(model);
        }

        private async Task SetPhoneNumberAsync(PasswordModel model)
        {
            var response = await NetService.ValidateVerifyCode(User.Id, EmailType.MobileBind, VerifyCode);
            if (response.IsSuccess)
            {
                var bindMobile = await userManager.SetPhoneNumberAsync(User, model.Mobile);
                if (bindMobile.Succeeded)
                {
                    ToastSuccess("绑定手机成功");
                    await LoadData();
                    UpdateUI();
                }
            }
            else
                ToastError(response.Message);
        }

        protected BZUserModel User;

        protected override async Task InitilizePageDataAsync() => await LoadData();

        private async Task LoadData()
        {
            User = await GetUser();
            value = new PasswordModel()
            {
                Mobile = User?.PhoneNumber ?? "",
                Code = ""
            };
            showInput = !string.IsNullOrWhiteSpace(value.Mobile);
        }
    }
}