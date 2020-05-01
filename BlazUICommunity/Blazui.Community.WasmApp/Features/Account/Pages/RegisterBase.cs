using Blazui.Community.WasmApp.Pages;
using Blazui.Community.DTO;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Blazui.Community.WasmApp.Service;
using Blazui.Community.Shared;

namespace Blazui.Community.WasmApp.Features.Account.Pages
{
    public class RegisterBase : PageBase
    {
        [Inject]
        public IAuthenticationService authenticationService { get; set; }

        protected BForm registerForm;
        internal RegisterAccountDto Value = new RegisterAccountDto();

        protected InputType passwordType { get; set; } = InputType.Password;

        internal void TogglePassword() => passwordType = passwordType == InputType.Text ? InputType.Password : InputType.Text;

        protected InputType ConfirmPasswordType { get; set; } = InputType.Password;

        internal void ToggleConfirmPassword() => ConfirmPasswordType = ConfirmPasswordType == InputType.Text ? InputType.Password : InputType.Text;

        protected async Task RegisterUser()
        {
            if (!registerForm.IsValid())
            {
                return;
            }
            await WithFullScreenLoading(async () =>
            {
                var registerAccountModel = registerForm.GetValue<RegisterAccountDto>();
                if (RegexHelper.ContainsChineseCharacters(registerAccountModel.UserAccount))
                {
                    ToastError("不支持中文账号");
                    return;
                }

                if (!registerAccountModel.Password.Equals(registerAccountModel.ConfirmPassword))
                {
                    ToastError("两次密码输入不一致");
                    return;
                }
                var RegistResult = await Regist(registerAccountModel);

                if (!RegistResult.Successful)
                {
                    foreach (var identityError in RegistResult.Errors)
                    {
                        await Task.Delay(100);
                        ToastError(identityError);
                    }
                    return;
                }
                else
                {
                    await AutoLogin(registerAccountModel);
                }
            });
        }

        private async Task<RegisterResult> Regist(RegisterAccountDto registerAccountModel)
        {
            var registModel = new RegisterModel()
            {
                Account = registerAccountModel.UserAccount,
                Password = registerAccountModel.Password
            };

            var RegistResult = await authenticationService.Register(registModel);
            return RegistResult;
        }

        private async Task AutoLogin(RegisterAccountDto registerAccountModel)
        {
            ToastSuccess("注册成功，即将自动登录...");
            var loginModel = new LoginModel
            {
                Password = registerAccountModel.Password,
                Username = registerAccountModel.UserAccount
            };
            var loginResult = await authenticationService.Login(loginModel);
            if (loginResult.Successful)
            {
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


    }
}