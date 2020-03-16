using Blazui.Community.Admin.Enum;
using Blazui.Community.Admin.Pages;
using Blazui.Community.Admin.Pages.Banner;
using Blazui.Community.Admin.QueryCondition;
using Blazui.Community.Admin.ViewModel;
using Blazui.Community.DTO;
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
    public class BannerManageBase : ManagePageBase<BannerAutoGenerateColumnsDto>
    {

        protected override async Task LoadDatas(bool MustRefresh = false)
        {
            var datas = await NetService.GetBanners(pageSize, currentPage);
            if (datas.IsSuccess)
            {
                var Items = datas.Data.Items.ToList();
                Datas = Mapper.Map<List<BannerAutoGenerateColumnsDto>>(Items);
                DataCount = datas.Data.TotalCount;
            }
            else
            {
                Datas = new List<BannerAutoGenerateColumnsDto>();
                DataCount = 0;
            }
        }

        protected async Task Modify(object obj)
        {
            if (obj is BannerAutoGenerateColumnsDto banner)
            {
                var model = banner.ObjectToDictionary("model");
                model.Add("EntryOperation", EntryOperation.Update);
                DialogResult result = await DialogService.ShowDialogAsync<ModifyBanner>("编辑Banner", 700, model);
                if (Convert.ToBoolean(result.Result))
                    await SearchData();
            
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
                await SearchData();
            }

        }

        protected async Task Delete(object obj)
        {
            if (obj is BannerAutoGenerateColumnsDto dto)
            {
                await ConfirmService.ConfirmAsync(
                    async () => await NetService.DeleteBanner(dto.Id),
                    async () => await SearchData(),
                "确定要删除？");
            }
        }
    }
}
