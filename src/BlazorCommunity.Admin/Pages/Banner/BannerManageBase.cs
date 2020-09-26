using BlazorCommunity.Admin.Enum;
using BlazorCommunity.Admin.QueryCondition;
using BlazorCommunity.Admin.Service;
using BlazorCommunity.DictionaryExtensions;
using BlazorCommunity.DTO.Admin;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorCommunity.Admin.Pages.Banner
{
    public class BannerManageBase : ManagePageBase<BannerDisplayDto>
    {
        [Inject]
        AdminUserService adminUserService { get; set; }
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
            if(!await adminUserService.IsSupperAdmin())
            {
                await MessageBox.AlertAsync("没有权限操作");
                return;
            }
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
            if (!await adminUserService.IsSupperAdmin())
            {
                await MessageBox.AlertAsync("没有权限操作");
                return;
            }
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