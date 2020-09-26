using Blazui.Community.DTO;
using Blazui.Community.Response;
using Blazui.Community.WasmApp.Model;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Blazui.Community.WasmApp.Pages.Mobile
{
    public partial class MobileReplyList : PageBase
    {
        internal IList<BZReplyDto> Replys { get; set; } = new List<BZReplyDto>();

        [Parameter]
        public BZTopicDto TopicModel { get; set; } = new BZTopicDto();
        protected bool IsEnd = false;

        protected int PageSize { get; set; } = 10;
        protected int currentPage = 1;


        [Parameter]
        public EventCallback OnItemDeleted { get; set; }

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

        [Parameter]
        public EventCallback OnNewReply { get; set; }

        /// <summary>
        /// 总数据条数
        /// </summary>
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
                LoadData();
            }
        }

        private async Task LoadData()
        {
            if (TopicModel == null)
            {
                ToastError("主贴不存在或已删除");
                await Task.Delay(500);
                NavigationManager.NavigateTo("/");
            }

            IsEnd = TopicModel.Status == 1;
            Total = 0;
            Replys = new List<BZReplyDto>();
            var result = await NetService.QueryReplysByTopicId(TopicModel.Id, currentPage, PageSize);
            if (result.IsSuccess)
                HandData(result);

            MarkAsRequireRender();
            StateHasChanged();
        }

        private void HandData(BaseResponse<PageDatas<BZReplyDto>> result)
        {
            foreach (var item in result.Data.Items)
            {
                item.OriginalContent = item.Content;
                item.IsMySelf = item.CreatorId == User?.Id;
                item.ShoudEdit = false;
            }
            Replys = result.Data.Items;
            Total = result.Data.TotalCount;
        }

    


        public NewReplyModel MBZReplyDto { get; set; }

    

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
            User = await GetUser();
            await LoadData();
        }

        protected override bool ShouldRender() => true;

        internal async Task OnReplySuccess()
        {
            if (OnNewReply.HasDelegate)
                await OnNewReply.InvokeAsync("");
            await LoadData();
        }

        internal void NavigateToLoginPage() => NavigationManager.NavigateTo("/account/signin?returnUrl=" + System.Net.WebUtility.UrlEncode(new Uri(NavigationManager.Uri).PathAndQuery));
    }
}
