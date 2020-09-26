using BlazorCommunity.DTO.App;
using BlazorCommunity.Model.Models;
using Element;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace BlazorCommunity.WasmApp.Components.User
{
    [Authorize]
    public partial class PersonalInfo : PersonalPageBase
    {
        protected BForm userInfoForm { get; set; }
        protected bool Disabled { get; set; } = true;

        protected void OnStatusChanging(BChangeEventArgs<RadioStatus> e)
        {
            e.DisallowChange = Disabled;
        }

        protected override async Task InitilizePageDataAsync()
        {
            User ??= await GetUser();
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
            RefreshUI();
            await Task.CompletedTask;
        }

        private void RefreshUI()
        {
            this.MarkAsRequireRender();
            StateHasChanged();
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
            var model = userInfoForm.GetValue<BZUserModel>();
            if ( SubstringCount( model.Signature.ToLower(), "upload/avator/") >1)
            {
                ToastError("只能上传一张图片");
                return;
            }
            await UpdateUser(model);
           await localStorage.RemoveItemAsync("CurrentUser");
        }

        /*
		 * / <summary>
		 * / 更新用户提交DB
		 * / </summary>
		 * / <returns></returns>
		 */

        private async Task UpdateUser(BZUserModel model)
        {
            var updateUser = new UpdateUserDto()
            {
                Id = model.Id,
                NickName = model.NickName,
                Sex = model.Sex,
                Signature = model.Signature
            };
            //user.Email = model.Email;
            //user.Avatar = model.Avatar;
            //user.Mobile = model.Mobile;
            //user.LastLoginAddr = model.LastLoginAddr;
            var result = await NetService.UpdateUserAsync(updateUser);

            if (result.IsSuccess)
            {
                await navigationToUpdateUserUI("/user/base");
            }
            else
            {
                ToastError("更新失败");
            }
        }

        protected override bool ShouldRender() => true;

        /// <summary>
        /// 计算字符串中子串出现的次数
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="substring">子串</param>
        /// <returns>出现的次数</returns>
        static int SubstringCount(string str, string substring)
        {
            if (str.Contains(substring))
            {
                string strReplaced = str.Replace(substring, "");
                return (str.Length - strReplaced.Length) / substring.Length;
            }

            return 0;
        }
    }
}