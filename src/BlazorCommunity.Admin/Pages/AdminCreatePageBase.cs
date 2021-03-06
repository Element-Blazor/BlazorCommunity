﻿using Element.Admin;
using BlazorCommunity.Admin.ViewModel;
using Element;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BlazorCommunity.Admin.Pages
{
    public class AdminCreatePageBase : BAdminPageBase
    {
        internal BForm form;

        [Parameter]
        public AdminLoginInfoModel DefaultUser { get; set; }

        [Inject]
        private RoleManager<IdentityRole> RoleManager { get; set; }

        //internal bool IsAdminDisable = true;

        protected override bool ShouldRender() => true;

        protected InputType passwordType { get; set; } = InputType.Password;

        internal void TogglePassword() => passwordType = passwordType == InputType.Text ? InputType.Password : InputType.Text;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                //IsAdminDisable = await RoleManager.RoleExistsAsync("管理员");
                form.MarkAsRequireRender();
                MarkAsRequireRender();
                StateHasChanged();
            }
        }

        public virtual async Task CreateAsync()
        {
            if (!form.IsValid())
            {
                return;
            }
            var model = form.GetValue<AdminLoginInfoModel>();
            var result = IsAdmin ?
                    await UserService.CreateUserAsync(new UserModel {  Username=model.Username,Password=model.Password }) :
                    await UserService.CreateSuperUserAsync(model.Username, model.Password);
            if (string.IsNullOrWhiteSpace(result))
            {
                await UserService.LoginAsync(form, model.Username, model.Password, string.Empty);
                return;
            }
            Toast(result);
        }
    }
}