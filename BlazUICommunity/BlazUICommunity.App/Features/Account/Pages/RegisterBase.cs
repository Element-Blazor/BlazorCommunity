using Blazui.Community.App.Pages;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using Blazui.Community.Repository;
using Blazui.Component;
using Blazui.Component.Form;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Features.Account.Pages
{
    public class RegisterBase : PageBase
    {
        [Inject]
         BZUserIdentityRepository _bZUserRepository { get; set; }

        protected BForm registerForm;
        internal RegisterAccountDto Value;
        protected async Task RegisterUser()
        {
            if ( !registerForm.IsValid() )
            {
                return;
            }
            var registerAccountModel = registerForm.GetValue<RegisterAccountDto>();
            if ( !registerAccountModel.Password.Equals(registerAccountModel.ConfirmPassword) )
            {
                MessageService.Show("两次密码输入不一致",MessageType.Error);
                return;
            }
            var user = await userManager.FindByEmailAsync(registerAccountModel.UserAccount);
            if ( user != null )
            {
                MessageService.Show("用户账号已存在", MessageType.Error);
            }
            var result = await _bZUserRepository.CreateUserAsync(registerAccountModel.UserAccount , registerAccountModel.Password);
            if ( result )
            {

                navigationManager.NavigateTo("account/signin" , true);
                return;
            }
            MessageService.Show("注册失败", MessageType.Error);
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
