﻿using Blazui.Community.Admin.Enum;
using Blazui.Community.Admin.Pages;
using Blazui.Community.Admin.Pages.Banner;
using Blazui.Community.Admin.QueryCondition;
using Blazui.Community.Admin.ViewModel;
using Blazui.Community.DTO;
using Blazui.Community.DTO.Admin;
using Blazui.Community.Request;
using Blazui.Community.Utility.Extensions;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Admin.Pages.Banner
{
    public class BannerManageBase : ManagePageBase<BannerDisplayDto>
    {
        protected override async Task OnInitializedAsync()
        {
            pageSize = 5;
            await base.OnInitializedAsync();
        }
        protected override async Task LoadDatas(bool MustRefresh = false)
        {
            var datas = await NetService.QueryBanners(new QueryBannerCondition() { PageSize = pageSize, PageIndex = currentPage }, MustRefresh);
            if (datas.IsSuccess)
                SetData(datas.Data.Items, datas.Data.TotalCount);
            else if (datas.Code == 204)
                SetData();
        }

        protected async Task Modify(object obj)
        {
            if (obj is BannerDisplayDto banner)
            {
                var model = banner.ObjectToDictionary("model");
                model.Add("EntryOperation", EntryOperation.Update);
                DialogResult result = await DialogService.ShowDialogAsync<ModifyBanner>("编辑Banner", 700, model);
                if (Convert.ToBoolean(result.Result))
                    await SearchData(true);

            }
        }
        protected async Task New()
        {

            var model = new Dictionary<string, object>
            {
                { "EntryOperation", EntryOperation.Add }
            };
            DialogResult result = await DialogService.ShowDialogAsync<ModifyBanner>("添加Banner", 700, model);
            if (Convert.ToBoolean(result.Result))
            {
                await SearchData(true);
            }

        }

        protected async Task Delete(object obj)
        {
            if (obj is BannerDisplayDto dto)
            {
                await ConfirmService.ConfirmAsync(
                    async () => await NetService.DeleteBanner(dto.Id),
                    async () => await SearchData(true),
                "确定要删除？");
            }
        }

        protected async Task Resume(object obj)
        {
            if (obj is BannerDisplayDto dto)
            {
                await ConfirmService.ConfirmAsync(
                    async () => await NetService.ResumeBanner(dto.Id),
                    async () => await SearchData(true),
                "确定要删除？");
            }
        }
    }
}
