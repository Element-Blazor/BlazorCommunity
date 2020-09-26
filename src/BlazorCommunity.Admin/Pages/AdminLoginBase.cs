using Blazui.Admin;
using Blazui.Component;
using Microsoft.AspNetCore.Components;

namespace BlazorCommunity.Admin.Pages
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