using Blazui.Community.DTO;
using Blazui.Community.WasmApp.Model.Condition;
using Blazui.Community.WasmApp.Pages;
using Blazui.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.WasmApp.Components.User
{
    public abstract class PersonalTopicTableBase<T>:PageBase
    {
        protected int currentPage = 1;
        protected int pageSize = 7;
        protected int DataCount = 0;
        protected IList<T> Datas = new List<T>();
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
                LoadDatas();
            }
        }

        protected async Task SearchData()
        {
            currentPage = 1;
            await table.WithLoadingAsync(async () => await LoadDatas());
        }

        protected abstract Task LoadDatas();

        protected void SetData(IList<T> datas = null, int? Total = 0)
        {
            datas ??= new List<T>();
            Datas = datas;
            DataCount = Total ?? 0;
            searchForm?.MarkAsRequireRender();
            table?.MarkAsRequireRender();
            StateHasChanged();
        }


        protected override async Task InitilizePageDataAsync() {

            User =await GetUser();
            await SearchData();
        }

        protected override bool ShouldRender() => true;

        protected void LinktoTopic(string topicId) => NavigationManager.NavigateTo($"/topic/{topicId}");


    }
}
