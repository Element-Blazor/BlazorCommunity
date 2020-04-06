using Blazui.Community.App.Pages;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using Blazui.Community.Response;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazui.Community.App.Components
{
    public class ReplyListBase : PageBase
    {
        internal IList<BZReplyDto> Replys { get; set; } = new List<BZReplyDto>();

        [Parameter]
        public string TopicId { get; set; }

        protected int PageSize { get; set; } = 10;
        protected int currentPage = 1;

        protected BZUserModel User;

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
            if (string.IsNullOrWhiteSpace(TopicId))
            {
                ToastError("主贴不存在或已删除");
                await Task.Delay(500);
                navigationManager.NavigateTo("/");
            }

            Total = 0;
            Replys = new List<BZReplyDto>();
            //await Task.Delay(1000);
            var result = await NetService.QueryReplysByTopicId(TopicId, currentPage, PageSize);
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

        protected async Task EditReply(BZReplyDto ReplyModel)
        {
            if (ReplyModel.ShoudEdit)
            {
                if (ReplyModel.OriginalContent != ReplyModel.Content)
                {
                    await WithFullScreenLoading(async () =>
                    {
                        var result = await NetService.UpdateReply(new BZReplyDto() { Id = ReplyModel.Id, Content = ReplyModel.Content, LastModifierId = User.Id });
                        MessageService.Show(result.IsSuccess ? "编辑成功" : "编辑失败", result.IsSuccess ? MessageType.Success : MessageType.Error);
                        if (result != null && result.IsSuccess)
                            await LoadData();
                    });
                }
            }
            ReplyModel.ShoudEdit = !ReplyModel.ShoudEdit;
        }

        /// <summary>
        /// 遗留问题，暂时只能直接刷新url
        /// </summary>
        /// <param name="replyId"></param>
        /// <returns></returns>
        protected async Task DeleteRep(string replyId)
        {
            MessageBoxResult Confirm = await MessageBox.ConfirmAsync("确定要删除？");
            if (Confirm == MessageBoxResult.Ok)
            {
                var result = await NetService.DeleteRelpy(replyId);
                if (result.IsSuccess)
                {
                    //if (OnItemDeleted.HasDelegate)
                    //    await OnItemDeleted.InvokeAsync(replyId);

                    //await LoadData();
                    //ToastSuccess("删除成功");
                    ////var deleteReply = Replys.FirstOrDefault(p => p.Id == replyId);
                    ////Replys.Remove(deleteReply);
                    //await Task.Delay(500);
                    navigationManager.NavigateTo(navigationManager.Uri, true);
                    //MarkAsRequireRender();
                    //StateHasChanged();
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

        internal void NavigateToLoginPage()=> navigationManager.NavigateTo("/account/signin?returnUrl=" + System.Net.WebUtility.UrlEncode(new Uri(navigationManager.Uri).PathAndQuery));
    }
}