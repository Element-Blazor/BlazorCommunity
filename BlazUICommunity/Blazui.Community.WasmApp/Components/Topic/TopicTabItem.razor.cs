using Blazui.Community.WasmApp.Pages;
using Blazui.Community.DTO;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.WasmApp.Components.Topic
{
    public partial class TopicTabItem : PageBase
    {
        protected List<BZTopicDto> Topics = new List<BZTopicDto>();

        protected int PageSize { get; set; } = 15;
        protected int currentPage = 1;
        internal bool requireRender = false;

        /// <summary>
        /// 排序类型 0-综合，1-人气，2-精华
        /// </summary>
        [Parameter]
        public int OrderType { get; set; } = 0;

        [Parameter]
        public int TopicType { get; set; } = -1;

        /// <summary>
        /// 当只有一页时，不显示分页
        /// </summary>

        public bool NoPaginationOnSinglePage { get; set; } = true;
        /// <summary>
        /// 最大显示的页码数
        /// </summary>

        public int ShowPageCount { get; set; } = 7;
        /// <summary>
        /// 当前最大显示的页码数变化时触发
        /// </summary>

        public EventCallback<int> ShowPageCountChanged { get; set; }

        /// <summary>
        /// 当页码变化时触发
        /// </summary>
        public EventCallback<int> CurrentPageChanged { get; set; }

        /// <summary>
        /// 当表格无数据时显示的消息
        /// </summary>

        public string EmptyMessage { get; set; }

        /// <summary>
        /// 总数据条数
        /// </summary>
        public int Total { get; set; } = 0;

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
                LoadData();
            }
        }

        private async Task LoadData()
        {
            var datas = await NetService.QueryTopicsByOrder(OrderType, TopicType, currentPage, PageSize);
            if (datas != null && datas.IsSuccess && datas.Data.Items != null)
            {
                Topics = datas.Data.Items.ToList();
                Total = datas.Data.TotalCount;
                StateHasChanged();
            }
            else if (datas.Code == 204)
            {
                Topics = new List<BZTopicDto>();
                Total = 0;
                StateHasChanged();
            }
        }

        internal async Task CurrentPageChangedAsync(int page)
        {
            CurrentPage = page;
            if (CurrentPageChanged.HasDelegate)
            {
                RequireRender = true;
                LoadingService.Show();
                await CurrentPageChanged.InvokeAsync(page);
                LoadingService.CloseFullScreenLoading();
            }
        }

        protected override async Task InitilizePageDataAsync()
        {
            Topics = new List<BZTopicDto>();
            Total = 0;
            await WithFullScreenLoading(async () =>
            {
                await LoadData();
            });
        }
    }
}