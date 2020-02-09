using Blazui.Community.App.Pages;
using Blazui.Community.Model.Models;
using Blazui.Component;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Components
{
    public class PersonalAvatorBase : PersonalPageBase
    {

        protected string UploadApiUrl { get; private set; }
        protected override async Task InitilizePageDataAsync()
        {
            await Task.CompletedTask;
        }
        protected override void OnInitialized()
        {

            UploadApiUrl = "api/upload/uploadavator";
            base.OnInitialized();

        }

        protected BZUserModel User { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            var userstatue = await authenticationStateTask;
            User = await userManager.GetUserAsync(userstatue.User);
            value = new UploadActivity()
            {
                Previews = new UploadModel[]
                 {
                 new UploadModel()
                {
                    FileName  = User?.Avatar,
                    Url  = User?.Avatar
                }
                 }
            };
        }

        protected BForm bForm;
        protected object value;
        protected async Task Submit()
        {
            if ( !bForm.IsValid() )
            {
                return;
            }
            var activity = bForm.GetValue<UploadActivity>();
            var upload = activity.Previews.FirstOrDefault();
            User.Avatar = upload?.Id;
           
           var identityResult= await userManager.UpdateAsync(User);
            if( identityResult.Succeeded )
            {
                await navigationToUpdateUserUI("/user/avator");
            }
        }

        protected override void InitTabTitle()
        {
            tabTitle = "我的头像";
        }
    }
    internal class UploadActivity
    {
        public IFileModel[] Previews { get; set; }

    }
}
