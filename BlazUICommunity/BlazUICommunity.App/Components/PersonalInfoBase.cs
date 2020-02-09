using Blazui.Community.App.Pages;
using Blazui.Community.Model.Models;
using Blazui.Component;
using Blazui.Component.EventArgs;
using Blazui.Component.Input;
using Blazui.Component.Radio;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Blazui.Community.App.Components
{
    public class PersonalInfoBase : PersonalPageBase
    {
        protected BForm userInfoForm { get; set; }
        protected BZUserModel User { get; set; }
        protected BInput<string> bInputSignature { get; set; }
        protected bool Disabled { get; set; } = true;
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                StateHasChanged();
            }
            else
            {
                return;
            }
        }
        protected void OnStatusChanging(BChangeEventArgs<RadioStatus> e)
        {
            e.DisallowChange = Disabled;
        }

        protected override async Task InitilizePageDataAsync()
        {
            var userstatue = await authenticationStateTask;
            User = await userManager.GetUserAsync(userstatue.User);
        }


        protected override void InitTabTitle()
        {
            tabTitle = "基本信息";
        }

        /*
          * / <summary>
          * / 切换表单为可输入状态
          * / </summary>
          */
        protected async Task EditUser()
        {
            Disabled = false;
            StateHasChanged();
             await   Task.CompletedTask;
        }

        /*
		 * / <summary>
		 * / 更新用户
		 * / </summary>
		 */
        protected async Task SaveUser()
        {
            if (!userInfoForm.IsValid())
                return;
            await UpdateUser();
        }


        /*
		 * / <summary>
		 * / 更新用户提交DB
		 * / </summary>
		 * / <returns></returns>
		 */
        private async Task UpdateUser()
        {
            var model = userInfoForm.GetValue<BZUserModel>();
            var user = await userManager.FindByNameAsync(model.UserName);

            user.NickName = model.NickName;
            user.Sex = model.Sex;
            user.Signature = model.Signature;
            //user.Email = model.Email;
            //user.Avatar = model.Avatar;
            //user.Mobile = model.Mobile;
            //user.LastLoginAddr = model.LastLoginAddr;
            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                //User = user;
                //Disabled = true;
                //this.RequireRender = true;
                //StateHasChanged();
               await navigationToUpdateUserUI("/user/base");
            }
            else
            {
                MessageService.Show("更新失败", MessageType.Error);
            }
        }

        protected override bool ShouldRender()
        {
            return true;
        }
    }


}
