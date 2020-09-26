using BlazorCommunity.DTO;
using BlazorCommunity.DTO.App;
using BlazorCommunity.Model.Models;
using Blazui.Component;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCommunity.WasmApp.Components.User
{
    [Authorize]
    public partial class PersonalAvator : PersonalPageBase
    {
        protected override bool ShouldRender() => true;


        protected override async Task InitilizePageDataAsync()
        {
            User = await GetUser();
            value = new UploadActivity()
            {
                Previews = new UploadModel[]
              {
                     new UploadModel(){
                        FileName  = User?.Avator,
                        Url  = User?.Avator
                    }
               }
            };
        }

        protected BForm bForm;
        protected UploadActivity value;

        protected async Task Submit()
        {
            if (!bForm.IsValid())
            {
                return;
            }
            var activity = bForm.GetValue<UploadActivity>();
            var upload = activity.Previews.FirstOrDefault();
            User.Avator = upload?.Url;

            var identityResult = await NetService.UpdateUserAvator(new UpdateUserAvatorDto { UserId=User.Id,AvatorUrl=upload.Url});
            if (identityResult.IsSuccess)
            {
                await navigationToUpdateUserUI("/user/avator");
            }
            else
            {
                ToastError("更新失败");
            }
        }

        protected override void InitTabTitle()
        {
            tabTitle = "我的头像";
        }
    }

    public class UploadActivity
    {
        public IFileModel[] Previews { get; set; }
    }
}