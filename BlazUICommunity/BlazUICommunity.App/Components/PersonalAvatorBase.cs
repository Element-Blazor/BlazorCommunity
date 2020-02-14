using Blazui.Community.App.Pages;
using Blazui.Community.Model.Models;
using Blazui.Component;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Components
{
    [Authorize]
    public class PersonalAvatorBase : PersonalPageBase
    {

        protected static string UploadApiUrl { get; private set; }

 
        protected override async Task InitilizePageDataAsync()
        {
            await Task.CompletedTask;
        }

        protected override void OnInitialized()
        {
            UploadApiUrl ??= "api/upload/uploadavator";
            base.OnInitialized();
        }

        protected BZUserModel User { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            User ??= await GetUser();
            value = new UploadActivity()
            {
                Previews = new UploadModel[]
              {
                 new UploadModel()
                {
                    FileName  = User?.Avator,
                    Url  = User?.Avator
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
            User.Avator = upload?.Id;
           
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
