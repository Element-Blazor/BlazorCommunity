using Blazui.Community.Admin.Enum;
using Blazui.Community.Admin.QueryCondition;
using Blazui.Community.Admin.ViewModel;
using Blazui.Community.Request;
using Blazui.Community.Utility.Extensions;
using Blazui.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Admin.Pages.Version
{
    public class VersionManageBase : ManagePageBase
    {
        protected int pageSize = 10;
        protected int currentPage = 1;
        internal bool requireRender = false;
        protected List<VersionAutoGenerateColumnsDto> Datas = new List<VersionAutoGenerateColumnsDto>();
        protected int DataCount = 5;
        protected BTable table;
        protected BForm searchForm;
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
            var condition = searchForm.GetValue<QueryVersionCondition>();
            var projectId = condition.ProjectId.HasValue ? (int)condition.ProjectId : -1;
            var datas = await NetService.GetVersions(new PageInfo() { PageIndex = currentPage, PageSize = pageSize }, projectId);
            if (datas.IsSuccess)
            {
                var data = datas.Data.Items.ToList();
                Datas = Mapper.Map<List<VersionAutoGenerateColumnsDto>>(data);
                DataCount = datas.Data.TotalCount;
            }
            else
            {
                Datas = new List<VersionAutoGenerateColumnsDto>();
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
            if (obj is VersionAutoGenerateColumnsDto version)
            {
                var model = version.ObjectToDictionary("model");
                model.Add("EntryOperation", EntryOperation.Update);
                DialogResult result = await DialogService.ShowDialogAsync<ModifyVersion>("编辑版本", 700, model);
                if (Convert.ToBoolean(result.Result))
                {
                    await SearchData();
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
                await SearchData();
            }

        }

        protected async Task Delete(object obj)
        {
            if (obj is VersionAutoGenerateColumnsDto dto)
            {
                await ConfirmAsync(
                    async () => await NetService.DeleteVersion(dto.Id),
                    async ()=>await SearchData(),
              "确定要删除？");
            }
        }

  
    }
}
