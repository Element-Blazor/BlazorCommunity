using Blazui.Community.App.Pages;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using Blazui.Community.Utility;
using Blazui.Community.Utility.Response;
using Blazui.Component;
using Blazui.Component.Pagination;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Blazui.Community.App.Components
{
    public class ReplyListBase : PageBase
    {
        [Parameter]
        public List<BZReplyDtoWithUser> Replys { get; set; }

        protected int PageSize { get; set; } = 5;
        protected int currentPage = 1;
        internal bool requireRender = false;
        protected BPagination Bpagination;
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
                requireRender = true;
                LoadData();
            }
        }

        private async Task LoadData()
        {
            var parsedQuery = HttpUtility.ParseQueryString(new Uri(navigationManager.Uri).Query);
            int.TryParse(parsedQuery["tpid"], out int TopicId);
            //int.TryParse(parsedQuery["golast"], out int Golast);


            if (TopicId < 0)
            {
                ToastError("topicid error");
                return;
            }
            var result = await NetService.GetReplys(TopicId, currentPage, PageSize);
            if (result != null && result.IsSuccess)
            {
                HandData(result);
            }
            else
            {
                Total = 0;
                Replys = new List<BZReplyDtoWithUser>();
            }

            MarkAsRequireRender();
            StateHasChanged();
        }

        private void HandData(BaseResponse<PageDatas<BZReplyDtoWithUser>> result)
        {
            Total = result.Data.TotalCount;
            Replys = result.Data.Items.ToList();
            foreach (var item in Replys)
            {
                if (item.ModifyTime != null)
                {
                    item.LastModifyTime = Convert.ToDateTime(item.ModifyTime).ConvertToDateDiffStr();
                }
            }
        }


        protected async Task DeleteRep(int replyId)
        {
            MessageBoxResult Confirm = await MessageBox.ConfirmAsync("确定要删除？");
            if (Confirm == MessageBoxResult.Ok)
            {
                var result = await NetService.DelRelpy(replyId);
                if (result.IsSuccess)
                {
                    ToastSuccess("删除成功");
                    await LoadData();
                }
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
            await LoadData();
            await Task.CompletedTask;
        }

        protected override bool ShouldRender() => true;
    }
}
