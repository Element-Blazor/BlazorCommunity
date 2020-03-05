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
    public class BannerManageBase : ManagePageBase
    {
        protected int pageSize = 10;
        protected int currentPage = 1;
        internal bool requireRender = false;
        protected List<BannerAutoGenerateColumnsDto> Datas = new List<BannerAutoGenerateColumnsDto>();
        protected int DataCount = 5;
        protected BTable table;
        protected BForm searchForm;
        [Inject]
        IConfiguration Configuration { get; set; }
        internal int CurrentPage
        {
            get
            {
                return currentPage;
            }
            set
            {
                currentPage = value;
                requireRender = true;
                SearchData();
            }
        }

        protected async Task SearchData()
        {
            await table.WithLoadingAsync(async () =>
            {
                await LoadDatas();
            });
            UpdateUI();
        }

        protected override async Task InitilizePageDataAsync()
        {
            await SearchData();
        }

        private async Task LoadDatas()
        {
            var datas = await NetService.GetBanners(new PageInfo() { PageIndex = currentPage, PageSize = pageSize });
            if (datas.IsSuccess)
            {
                var Items = datas.Data.Items.ToList();
                Datas = Mapper.Map<List<BannerAutoGenerateColumnsDto>>(Items);
                DataCount = datas.Data.TotalCount;
                //Datas.ForEach(p => p.BannerImg = Configuration["ServerUrl"] + "/"+ p.BannerImg);
            }
            else
            {
                Datas = new List<BannerAutoGenerateColumnsDto>();
                DataCount = 0;
            }
        }


        private void UpdateUI()
        {
            requireRender = true;
            searchForm?.MarkAsRequireRender();
            table?.MarkAsRequireRender();
            table?.Refresh();
            StateHasChanged();
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
                await ConfirmAsync(
                    async () => await NetService.DeleteBanner(dto.Id),
                    async () => await SearchData(),
                "确定要删除？");
            }
        }
    }
}
