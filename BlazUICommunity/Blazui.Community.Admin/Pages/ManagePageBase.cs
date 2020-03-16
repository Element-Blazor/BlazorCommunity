using AutoMapper;
using Blazui.Community.Admin.QueryCondition;
using Blazui.Community.Admin.Service;
using Blazui.Community.Utility.Response;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Admin.Pages
{
    public abstract class ManagePageBase<T> : BComponentBase
    {

        protected int currentPage = 1;
        protected int pageSize = 10;
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
                SearchData();
            }
        }

        protected  async Task SearchData(bool MustRefresh = false)
        {
            currentPage = 1;
            await table.WithLoadingAsync(async () => await LoadDatas(MustRefresh));
        }
        protected abstract Task LoadDatas(bool MustRefresh = false);

        protected void SetData(IList<T> datas = null, int count = 0)
        {
            datas ??= new List<T>();
            Datas = datas;
            DataCount = count;

            searchForm?.MarkAsRequireRender();
            table?.MarkAsRequireRender();
            StateHasChanged();
        }

        protected TCondition BuildCondition<TCondition>() where TCondition : QueryBaseCondition,new()
        {
            TCondition condition = searchForm.GetValue<TCondition>();
            condition ??= new TCondition();
            condition.PageIndex = currentPage;
            condition.PageSize = pageSize;
            return condition;
        }

        [Inject]
        public ILogger<ManagePageBase<T>> _logger { get; set; }
        [Inject]
        public IMemoryCache MemoryCache { get; set; }
        [Inject]
        public NetworkService NetService { get; set; }

        [Inject]
        public MessageService MessageService { get; set; }
        [Inject]
        public MessageBox MessageBox { get; set; }
        [Inject]
        public ConfirmService ConfirmService { get; set; }
        [Inject]
        public IMapper Mapper { get; set; }




        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (!firstRender)
                return;
            try
            {
                await InitilizePageDataAsync();
                RequireRender = true;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"OnAfterRenderAsync----->>{ex.StackTrace}");
            }
        }

        protected virtual Task InitilizePageDataAsync() =>  SearchData();
        protected override bool ShouldRender() => true;


      
    }
}
