using Blazui.Community.Admin.Enum;
using Blazui.Community.Admin.Service;
using Blazui.Community.DTO.Admin;
using Blazui.Community.Enums;
using Blazui.Community.Response;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Admin.Pages.Banner
{
    public class ModifyBannerBase : BDialogBase
    {
        internal BForm versionForm;

        [Parameter]
        public BannerDisplayDto model { get; set; }

        [Parameter]
        public EntryOperation EntryOperation { get; set; }

        [Inject]
        private NetworkService NetService { get; set; }

        [Inject]
        private IConfiguration Configuration { get; set; }

        internal string ServerUrl { get; private set; }

        [Inject]
        private MessageService MessageService { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            ServerUrl = Configuration["ServerUrl"] + "/api/upload/" + UploadPath.Banner.Description();
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (EntryOperation == EntryOperation.Update)
                model.Previews = new UploadModel[] { new UploadModel() { FileName = model.BannerImg.Split("/").Last(), Url = model.BannerImg, Id = model.BannerImg, Status = 0 } };
        }

        internal async Task Save()
        {
            if (!versionForm.IsValid())
                return;
            var banner = versionForm.GetValue<BannerDisplayDto>();
            if (banner.Previews == null|| banner.Previews.Length == 0)
            {
                MessageService.Show("请上传图片后再提交", MessageType.Error);
                return;
            }
            if (banner.Previews.Length > 1)
            {
                MessageService.Show("一次只能上传一张图片", MessageType.Error);
                return;
            }
          
            banner.BannerImg = ((IFileModel[])banner.Previews).FirstOrDefault().Url;
            BaseResponse response;

            banner.LastModifyDate = DateTime.Now;
            banner.LastModifierId = Guid.Empty.ToString();
            if (EntryOperation == EntryOperation.Add)
            {
                banner.CreatorId = Guid.Empty.ToString();
                banner.CreateDate = DateTime.Now;
                response = await NetService.NewBanner(banner);
            }
            else
            {
                response = await NetService.UpdateBanner(banner);
            }
            if (!response.IsSuccess)
                MessageService.Show(response.Message, MessageType.Error);
            await CloseAsync(response.IsSuccess);
        }
    }
}