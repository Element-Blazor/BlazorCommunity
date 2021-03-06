﻿using BlazorCommunity.App.Model;
using BlazorCommunity.App.Pages;
using BlazorCommunity.Common;
using BlazorCommunity.Enums;
using BlazorCommunity.Model.Models;
using BlazorCommunity.Response;
using Element;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlazorCommunity.App.Components
{
    [Authorize]
    public class BindEmailBase : PageBase

    {
        protected BForm bForm { get; set; }
        internal PasswordModel value { get; set; }
        protected bool showInput { get; set; } = false;
        protected BCard bCard { get; set; }
        protected bool BtnBindEmailDisabled  = false;
        protected bool SendCanCelEmailBindCodeSuccess = false;
        protected bool BtnCancelDisabled = false;
        protected bool BtnConfirmDisabled = false;
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

        protected async Task CanCelBind()
        {
            BtnCancelDisabled = true;
           var result= await NetService.SendVerifyCode(User.Id, EmailType.EmailUnBind, User.Email);
            BtnCancelDisabled = false;

            SendCanCelEmailBindCodeSuccess = result.IsSuccess;
            if (!result.IsSuccess)
                ToastError(result.Message);
            VerifyCode = result.IsSuccess ? result.Data.ToString() : "";
            UpdateUI();
            
        }

        protected async Task CanCelBindConfirm()
        {
            var model = bForm.GetValue<PasswordModel>();

            var result = await NetService.ValidateVerifyCode(User.Id, EmailType.EmailUnBind, model.Code);
            if (result.IsSuccess)
            {
                User.Email = "";
                User.NormalizedEmail = "";
                var UplodateResult = await userManager.UpdateAsync(User);
                if (UplodateResult.Succeeded)
                {
                    ToastSuccess("邮箱已解绑");
                    TimeOut = 0;
                    await LoadData();
                    BtnBindEmailDisabled = false;
                    SendCanCelEmailBindCodeSuccess = false;
                    UpdateUI();
                }
                else
                    ToastError(JsonConvert.SerializeObject(UplodateResult.Errors));
            }
            else
                ToastError("验证失败");
        }
        protected override bool ShouldRender() => true;

        protected string VerifyCode = "";

        protected async Task SendEmailMsg()
        {
            if (!bForm.IsValid())
                return;
            var model = bForm.GetValue<PasswordModel>();
            var mobileValid = RegexHelper.IsEmail(model.Email);
            if (!mobileValid)
            {
                ToastError("邮箱号码错误");
                return;
            }
            await Wating(await NetService.SendVerifyCode(User.Id, EmailType.EmailBind, model.Email));
        }

        private async Task Wating(BaseResponse result)
        {
            if (result.IsSuccess)
            {
                ToastSuccess("验证码发送成功，2分钟内有效，请前往邮箱查收");
                VerifyCode = result.Data.ToString();
                BtnBindEmailDisabled = true;
                showInput = true;
                while (TimeOut > 0)
                {
                    if (TimeOut == 1)
                    {
                        BtnBindEmailDisabled = false;
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
            var result = await NetService.ValidateVerifyCode(User.Id, EmailType.EmailBind, VerifyCode);
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