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
         BZUserIdentityRepository _bZUserRepository { get; set; }

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
                MessageService.Show("不支持中文账号", MessageType.Error);
                return;
            }

            if (!registerAccountModel.Password.Equals(registerAccountModel.ConfirmPassword))
            {
                MessageService.Show("两次密码输入不一致", MessageType.Error);
                return;
            }
            var user = await userManager.FindByNameAsync(registerAccountModel.UserAccount);
            if (user != null)
            {
                MessageService.Show("用户账号已存在", MessageType.Error);
            }
            var result = await _bZUserRepository.CreateUserAsync(registerAccountModel.UserAccount, registerAccountModel.Password);
            if (result)
            {

                navigationManager.NavigateTo("account/signin", true);
                return;
            }
            MessageService.Show("注册失败", MessageType.Error);
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
