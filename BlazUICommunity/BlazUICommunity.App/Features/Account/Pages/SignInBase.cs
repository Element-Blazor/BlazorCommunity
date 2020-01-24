using Blazui.Component;
using Blazui.Component.Form;
using Blazui.Component.Popup;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazUICommunity.App.Features.Account.Pages
{
    public partial class SignInBase: ComponentBase
    {
        [Inject]
        UserManager<IdentityUser> userManager { get; set; }
        [Inject]
        SignInManager<IdentityUser> signInManager { get; set; }
        [Inject]
        NavigationManager navigationManager { get; set; }
        [Inject]
        IDataProtectionProvider dataProtectionProvider { get; set; }
        [Inject]
       public MessageService MessageService { get; set; }
        //[Inject]
        //BPopup pop { get; set; }

        [Parameter]
        public SignInModel signInModel { get; set; }
        private bool showSignInError = false;
        public static BForm registerForm;

      
        protected async Task Login()
        {
            var signInModel = registerForm.GetValue<SignInModel>();
            var user = await userManager.FindByEmailAsync(signInModel.Email);

            if ( user != null && await userManager.CheckPasswordAsync(user , signInModel.Password) )
            {
                showSignInError = false;

                var token = await userManager.GenerateUserTokenAsync(user , TokenOptions.DefaultProvider , "SignIn");

                var data = $"{user.Id}|{token}";

                var parsedQuery = System.Web.HttpUtility.ParseQueryString(new Uri(navigationManager.Uri).Query);

                var returnUrl = parsedQuery["returnUrl"];

                if ( !string.IsNullOrWhiteSpace(returnUrl) )
                {
                    data += $"|{returnUrl}";
                }

                var protector = dataProtectionProvider.CreateProtector("SignIn");

                var pdata = protector.Protect(data);

                navigationManager.NavigateTo("/account/signinactual?t=" + pdata , forceLoad: true);
            }
            else
            {
                showSignInError = true;
            }
            if ( showSignInError )
            {
                MessageService.Show("登录失败，用户名或密码错误" , MessageType.Error);
          
                return;
            }
        }

   
    }
    public class SignInModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
