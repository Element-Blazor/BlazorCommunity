using Blazui.Community.App.Model;
using Blazui.Community.App.Pages;
using Blazui.Community.Enums;
using Blazui.Community.Model.Models;
using Blazui.Community.Response;
using Blazui.Component;
using Blazui.Component.Container;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Blazui.Community.App.Components
{
    [Authorize]
    public class BindEmailBase : PageBase

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

        protected string VerifyCode = "";

        protected async Task SendEmailMsg()
        {
            if (!bForm.IsValid())
                return;
            var model = bForm.GetValue<PasswordModel>();
            var mobileValid = Regex.Match(model.Email, "^[a-z0-9]+([._\\-]*[a-z0-9])*@([a-z0-9]+[-a-z0-9]*[a-z0-9]+.){1,63}[a-z0-9]+$").Success;
            if (!mobileValid)
            {
                ToastError("邮箱号码错误");
                return;
            }
            await Wating(await NetService.SendVerifyCode(User.Id, VerifyCodeType.EmailBind, model.Email));
        }

        private async Task Wating(BaseResponse result)
        {
            if (result.IsSuccess)
            {
                ToastSuccess("验证码发送成功，2分钟内有效，请前往邮箱查收");
                VerifyCode = result.Data.ToString();
                IsDisabled = true;
                showInput = true;
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
                ToastError(result.Message);
        }

        protected async Task CheckVerifyCode()
        {
            var model = bForm.GetValue<PasswordModel>();

            if (VerifyCode != model.Code)
            {
                ToastError("验证码无效");
                return;
            }
            var result = await NetService.ValidateVerifyCode(User.Id, VerifyCodeType.EmailBind, VerifyCode);
            await SetEmailAsync(model, result);
        }

        private async Task SetEmailAsync(PasswordModel model, BaseResponse response)
        {
            if (response.IsSuccess)
            {
                var bindMobile = await userManager.SetEmailAsync(User, model.Email);
                if (bindMobile.Succeeded)
                {
                    ToastSuccess("绑定邮箱成功");
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
                Email = User?.Email ?? "",
                Code = ""
            };
            showInput = !string.IsNullOrWhiteSpace(value.Email);
        }
    }
}