using BlazorCommunity.DTO;
using Element;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorCommunity.App.Components.Topic
{
    public class SeachTopicContainerBase : ElementComponentBase, IContainerComponent
    {
        [Parameter]
        public List<SeachTopicDto> Datas { get; set; }

        [Parameter]
        public int PageSize { get; set; } = 5;

        [Parameter]
        public int currentPage { get; set; } = 1;

        private string TopicId { get; set; }

        /// <summary>
        /// 当只有一页时，不显示分页
        /// </summary>
        [Parameter]
        public bool NoPaginationOnSinglePage { get; set; } = true;

        /// <summary>
        /// 最大显示的页码数
        /// </summary>
        [Parameter]
        public int ShowPageCount { get; set; } = 7;

        /// <summary>
        /// 当前最大显示的页码数变化时触发
        /// </summary>
        [Parameter]
        public EventCallback<int> ShowPageCountChanged { get; set; }

        /// <summary>
        /// 当页码变化时触发
        /// </summary>
        [Parameter]
        public EventCallback<int> CurrentPageChanged { get; set; }

        /// <summary>
        /// 当页码变化时触发
        /// </summary>
        [Parameter]
        public EventCallback<string> OnItemClick { get; set; }

        /// <summary>
        /// 当表格无数据时显示的消息
        /// </summary>

        public string EmptyMessage { get; set; }

        /// <summary>
        /// 总数据条数
        /// </summary>
        [Parameter]
        public int Total { get; set; }

        internal int CurrentPage
        {
            get
            {
                return currentPage;
            }
            set
            {
                currentPage = value;
            }
        }

        public ElementReference Container { get; set; }

        internal async Task ItemClick(string Id)
        {
            if (OnItemClick.HasDelegate)
                await OnItemClick.InvokeAsync(Id);
        }

        internal async Task CurrentPageChangedAsync(int page)
        {
            currentPage = page;
            if (CurrentPageChanged.HasDelegate)
            {
                RequireRender = true;
                LoadingService.Show();
                await CurrentPageChanged.InvokeAsync(page);
                LoadingService.CloseFullScreenLoading();
            }
        }

        protected override bool ShouldRender() => true;
    }
}