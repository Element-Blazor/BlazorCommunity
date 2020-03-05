using BlazAdmin;
using Blazui.Component;
using Blazui.Component.Input;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazAdmin
{
    public class AdminLoginBase : BAdminPageBase
    {
        public BForm Form { get; internal set; }
        protected override bool ShouldRender() => true;

        protected InputType passwordType = InputType.Password;
        internal void TogglePassword()
        {
            passwordType = passwordType == InputType.Text ? InputType.Password : InputType.Text;
        }

        public virtual async System.Threading.Tasks.Task LoginAsync()
        {
            if (!Form.IsValid())
            {
                return;
            }
            var model = Form.GetValue<LoginInfoModel>();
            var result = await UserService.CheckPasswordAsync(model.Username, model.Password);
            if (string.IsNullOrWhiteSpace(result))
            {
                await UserService.LoginAsync(Form, model.Username, model.Password, NavigationManager.Uri);
                return;
            }
            Toast(result);
        }
    }
}
