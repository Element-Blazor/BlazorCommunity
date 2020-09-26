using BlazorCommunity.Admin.QueryCondition;
using BlazorCommunity.Admin.Service;
using Element;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorCommunity.Admin.Pages
{
    public abstract class ManagePageBase<T> : ElementComponentBase
    {
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
                LoadDatas();
            }
        }

        protected async Task SearchData(bool MustRefresh = false)
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

        protected TCondition BuildCondition<TCondition>() where TCondition : BaseQueryCondition, new()
        {
            TCondition condition = searchForm.GetValue<TCondition>();
            condition ??= new TCondition();
            condition.PageIndex = currentPage;
            condition.PageSize = pageSize;
            return condition;
        }

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

        protected virtual async Task InitilizePageDataAsync() => await SearchData(true);

        protected override bool ShouldRender() => true;
    }
}