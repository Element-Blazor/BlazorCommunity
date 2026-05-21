using BlazorCommunity.App.Model;
using BlazorCommunity.App.Pages;
using BlazorCommunity.Model.Models;
using Element;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace BlazorCommunity.App.Components.User
{
    public class ChangePasswordByOldPasswordBase : PageBase
    {
        protected ElForm bformOldpwd;
        protected BZUserModel User;
        protected string VerifyCode = "";

        protected async Task ChangePwdByOld()
        {
            if (!bformOldpwd.IsValid())
                return;
            User = await GetUser();
            if (!await CheckOldPassword())
                return;
            if (!CheckConfirmPassword())
                return;
            await ResetPassword(bformOldpwd);
        }

        private async Task<bool> CheckOldPassword()
        {
            var model = bformOldpwd.GetValue<PasswordModel>();
            var checkOld = await userManager.CheckPasswordAsync(User, model.OldPassword);
            if (!checkOld)
            {
                ToastError("当前密码错误");
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

        private async Task ResetPassword(ElForm ElForm)
        {
            var PasswordModel = ElForm.GetValue<PasswordModel>();
            var token = await userManager.GeneratePasswordResetTokenAsync(User);
            var result = await userManager.ResetPasswordAsync(User, token, PasswordModel.Password);
            if (result.Succeeded)
            {
                ToastSuccess("密码重置成功,下次登录请使用新密码");
                ElForm.Reset();
                return;
            }
            else
                ToastError(JsonConvert.SerializeObject(result.Errors));
        }
    }
}