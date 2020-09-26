using BlazorCommunity.Shared;
using BlazorCommunity.WasmApp.Pages;
using BlazorCommunity.WasmApp.Service;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace BlazorCommunity.WasmApp.Features.Account.Pages
{
    public partial class SignIn : PageBase
    {
        [Inject]
        public IAuthenticationService authenticationService { get; set; }

        protected InputType passwordType { get; set; } = InputType.Password;

        [Parameter]
        public SignInModel signInModel { get; set; }

        public BForm signInForm;

        protected async Task Login()
        {
            if (!signInForm.IsValid())
                return;
            await WithFullScreenLoading(async () =>
            {
                try
                {
                    var signInModel = signInForm.GetValue<SignInModel>();
                    var loginModel = new LoginModel { Username = signInModel.UserAccount, Password = signInModel.Password };
                    var loginResult = await authenticationService.Login(loginModel);
                    if (loginResult.Successful)
                    {
                        ToastSuccess("登录成功");
                        await  NavigateToReturnUrl();
                    }
                    else
                    {
                        foreach (var item in loginResult.Errors)
                        {
                            ToastError(item);
                        }
                    }

                }
                catch (Exception ex)
                {
                    ToastError("登录失败" + ex.StackTrace);
                }
            });
        }

        internal void TogglePassword() => passwordType = passwordType == InputType.Text ? InputType.Password : InputType.Text;

        protected void Regist() => NavigationManager.NavigateTo("/account/register", true);

        protected void ForgetPwd() => NavigationManager.NavigateTo("/account/forget", true);


    }

    public class SignInModel
    {
        [Required]
        public string UserAccount { get; set; }

        [Required]
        public string Password { get; set; }
    }
}