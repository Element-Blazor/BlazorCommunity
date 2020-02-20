using Blazui.Community.App.Pages;
using Blazui.Community.DTO;
using Blazui.Community.Repository;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Blazui.Community.App.Features.Account.Pages
{
    public class RegisterBase : PageBase
    {
        [Inject]
         BZUserIdentityRepository BZUserRepository { get; set; }

        protected BForm registerForm;
        internal RegisterAccountDto Value;
        readonly string CheckChinaPattern = @"[\u4e00-\u9fa5]";//检查汉字的正则表达式
        protected async Task RegisterUser()
        {
            if (!registerForm.IsValid())
            {
                return;
            }
            var registerAccountModel = registerForm.GetValue<RegisterAccountDto>();
            if (ContainsChineseCharacters(registerAccountModel.UserAccount))
            {
                ToastError("不支持中文账号");
                return;
            }

            if (!registerAccountModel.Password.Equals(registerAccountModel.ConfirmPassword))
            {
                ToastError("两次密码输入不一致");
                return;
            }
            var user = await userManager.FindByNameAsync(registerAccountModel.UserAccount);
            if (user != null)
            {
                ToastError("用户账号已存在");
                return;
            }
            var result = await BZUserRepository.CreateUserAsync(registerAccountModel.UserAccount, registerAccountModel.Password);
            if (result)
            {
                navigationManager.NavigateTo("account/signin", true);
                return;
            }
            ToastError("注册失败");
        }

        private bool ContainsChineseCharacters(string input)
        {
            return Regex.Matches(input, CheckChinaPattern)?.Count>0;
          
        }

        protected override Task OnParametersSetAsync()
        {
            return base.OnParametersSetAsync();
        }
        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
        }

        protected override void OnParametersSet()
        {
            Value = new RegisterAccountDto();
            base.OnParametersSet();

        }

        protected override Task InitilizePageDataAsync()
        {
            return Task.CompletedTask;
        }
    }
}
