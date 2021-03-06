﻿using BlazorCommunity.Admin.Enum;
using BlazorCommunity.Admin.QueryCondition;
using BlazorCommunity.DictionaryExtensions;
using BlazorCommunity.DTO.Admin;
using Element;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorCommunity.Admin.Pages.Version
{
    public class VersionManageBase : ManagePageBase<VersionDisplayDto>
    {
        protected override async Task LoadDatas(bool MustRefresh = false)
        {
            var datas = await NetService.QueryVersions(BuildCondition<QueryVersionCondition>(), MustRefresh);
            if (datas.IsSuccess)
                SetData(datas.Data.Items, datas.Data.TotalCount);
            else if (datas.Code == 204)
                SetData();
        }

        protected async Task Modify(object obj)
        {
            if (obj is VersionDisplayDto version)
            {
                var model = version.ObjectToDictionary("model");
                model.Add("EntryOperation", EntryOperation.Update);
                DialogResult result = await DialogService.ShowDialogAsync<ModifyVersion>("编辑版本", 700, model);
                if (Convert.ToBoolean(result.Result))
                {
                    await SearchData(true);
                }
            }
        }

        protected async Task New()
        {
            var model = new Dictionary<string, object>
            {
                { "EntryOperation", EntryOperation.Add }
            };
            DialogResult result = await DialogService.ShowDialogAsync<ModifyVersion>("发布新版本", 700, model);
            if (Convert.ToBoolean(result.Result))
            {
                await SearchData(true);
            }
        }

        protected async Task Delete(object obj)
        {
            if (obj is VersionDisplayDto dto)
            {
                await ConfirmService.ConfirmAsync(
                    async () => await NetService.DeleteVersion(dto.Id),
                    async () => await SearchData(true),
              "确定要删除？");
            }
        }

        protected async Task Resume(object obj)
        {
            if (obj is VersionDisplayDto dto)
            {
                await ConfirmService.ConfirmAsync(
                    async () => await NetService.ResumeVersion(dto.Id),
                    async () => await SearchData(true),
              "确定要删除？");
            }
        }
    }
}