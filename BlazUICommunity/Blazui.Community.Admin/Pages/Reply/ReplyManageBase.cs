using Blazui.Community.Admin.Pages;
using Blazui.Community.Admin.Pages.Topic;
using Blazui.Community.Admin.QueryCondition;
using Blazui.Community.DTO;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Admin.Pages.Reply
{
    public class ReplyManageBase : ManagePageBase
    {
        protected int pageSize = 10;
        protected int currentPage = 1;
        internal bool requireRender = false;
        protected List<BZReplyDto> Datas = new List<BZReplyDto>();
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
            BuildCondition(out QueryReplyCondition condition, out object username, out object topicTitle);
            var datas = await NetService.QueryReplys(condition, username.ToString(), topicTitle.ToString());
            if (datas.IsSuccess)
            {
                Datas = datas.Data.Items.ToList();
                DataCount = datas.Data.TotalCount;
            }
            else
            {
                Datas = new List<BZReplyDto>();
                DataCount = 0;
            }
        }

        private void BuildCondition(out QueryReplyCondition condition, out object username, out object topicTitle)
        {
            condition = searchForm.GetValue<QueryReplyCondition>();
            condition ??= new QueryReplyCondition();
            username = condition.UserId ?? "";
            topicTitle = condition.TopicId ?? "";
            condition.UserId = null;
            condition.TopicId = null;
            condition.PageInfo.PageIndex = currentPage;
            condition.PageInfo.PageSize = pageSize;
        }

        private void UpdateUI()
        {
            requireRender = true;
            searchForm?.MarkAsRequireRender();
            table?.MarkAsRequireRender();
            table?.Refresh();
            StateHasChanged();
        }


        protected async Task Delete(object obj)
        {
            if (obj is BZReplyDto dtoWithUser)
            {
                await ConfirmAsync(
                    async () => await NetService.DelReply(dtoWithUser.Id),
                    async () => await SearchData());
            }

        }
        protected async Task Detail(object obj)
        {
            if (obj is BZReplyDto dto)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "Content", dto.Content }
                };
                await DialogService.ShowDialogAsync<TopicDeatil>("回贴内容", parameters);
            }
        }
    }
}
