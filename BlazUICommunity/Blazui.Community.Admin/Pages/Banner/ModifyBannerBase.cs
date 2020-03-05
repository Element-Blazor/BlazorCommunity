﻿using AutoMapper;
using Blazui.Community.Admin.Enum;
using Blazui.Community.Admin.Service;
using Blazui.Community.Admin.ViewModel;
using Blazui.Community.DTO;
using Blazui.Community.Enums;
using Blazui.Community.Utility.Response;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Admin.Pages.Banner
{
    public class ModifyBannerBase : BDialogBase
    {
        internal BForm versionForm;

        [Parameter]
        public BannerAutoGenerateColumnsDto model { get; set; }
        [Parameter]
        public EntryOperation EntryOperation { get; set; }
        [Inject]
        NetworkService NetService { get; set; }
        [Inject]
        IConfiguration Configuration { get; set; }
        internal string ServerUrl { get; private set; }
        [Inject]
        public IMapper Mapper { get; set; }

        [Inject]
        MessageService MessageService { get; set; }
        protected override void OnInitialized()
        {
            base.OnInitialized();
            ServerUrl = Configuration["ServerUrl"] + "/api/upload/" + UploadPath.Banner.Description();
        }
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (EntryOperation == EntryOperation.Update)
                model.Previews = new UploadModel[] { new UploadModel() { FileName = model.BannerImg.Split("/").Last(), Url = model.BannerImg, Id = model.BannerImg, Status = 0 } };
        }
        internal async Task Save()
        {
            if (!versionForm.IsValid())
                return;
            var banner = versionForm.GetValue<BannerAutoGenerateColumnsDto>();
            if (banner.Previews.Length > 1)
            {
                MessageService.Show("一次只能上传一张图片，请删除多余的", MessageType.Error);
                return;
            }
            banner.BannerImg = banner.Previews.FirstOrDefault().Url;
            var dto = Mapper.Map<BzBannerDto>(banner);
            BaseResponse response;
            if (EntryOperation == EntryOperation.Add)
            {
                dto.CreateDate = DateTime.Now;
                dto.LastModifyDate = DateTime.Now;
                response = await NetService.NewBanner(dto);
            }
            else
            {
                dto.LastModifyDate = DateTime.Now;
                response = await NetService.UpdateBanner(dto);
            }
            if (!response.IsSuccess)
                MessageService.Show(response.Message,MessageType.Error);
            await CloseAsync(response.IsSuccess);

        }
    }
}
