using Blazui.Community.WasmApp.Model;
using Blazui.Community.WasmApp.Pages;
using Blazui.Community.Model.Models;
using Blazui.Component;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Blazui.Community.DTO.App;

namespace Blazui.Community.WasmApp.Components.User
{
    public partial class ChangePasswordByOldPassword : PageBase
    {
        protected BForm bformOldpwd;
        protected string VerifyCode = "";

        protected async Task ChangePwdByOld()
        {
            if (!bformOldpwd.IsValid())
                return;
            User = await GetUser();
            //if (!await CheckOldPassword())
            //    return;
            if (!CheckConfirmPassword())
                return;
            await ResetPassword(bformOldpwd);
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
            var ResetPasswordResult = await NetService.ResetPasswordAsync(new UpdateUserPasswordDto { NewPassword = PasswordModel.Password, UserId = User.Id });
            if (ResetPasswordResult.IsSuccess)
            {
                ToastSuccess("密码重置成功,下次登录请使用新密码");
                bForm.Reset();
                return;
            }
            else
                ToastError(ResetPasswordResult.Message);
        }
    }
}