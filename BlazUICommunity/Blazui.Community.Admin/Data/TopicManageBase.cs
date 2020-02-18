using Blazui.Community.Admin.Pages;
using Blazui.Community.Admin.QueryCondition;
using Blazui.Community.DTO;
using Blazui.Component;
using Blazui.Component.Form;
using Blazui.Component.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Admin.Data
{
    public class TopicManageBase : ManagePageBase
    {
        protected int pageSize = 6;
        protected int currentPage = 1;
        internal bool requireRender = false;
        protected List<BZTopicDtoWithUser> Datas = new List<BZTopicDtoWithUser>();
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
            BuildCondition(out QueryTopicCondition condition, out string username);
            var datas = await NetService.QueryTopics(condition, username);
            if (datas.IsSuccess)
            {
                Datas = datas.Data.Items.ToList();
                DataCount = datas.Data.TotalCount;
            }
            else
            {
                Datas = new List<BZTopicDtoWithUser>();
                DataCount = 0;
            }
        }

        private void BuildCondition(out QueryTopicCondition condition, out string username)
        {
            condition = searchForm.GetValue<QueryTopicCondition>();
            condition ??= new QueryTopicCondition();
            username = "";
            if (condition.UserId != null)
                username = condition.UserId.ToString();
            condition.UserId = null;
        }

        private void UpdateUI()
        {
            requireRender = true;
            searchForm?.MarkAsRequireRender();
            table?.MarkAsRequireRender();
            table?.Refresh();
            StateHasChanged();
        }


        protected async Task Top(object obj)
        {

            if (obj is BZTopicDtoWithUser dto)
            {
                await ConfirmAsync(async () =>
                {
                    await NetService.TopTopic(dto.Id);
                    MessageService.Show(dto.Top==0?"置顶成功":"取消成功", MessageType.Success);
                    await SearchData();
                });
            }
        }
        protected async Task Detail(object obj)
        {
            if (obj is BZTopicDtoWithUser dto)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "Content", dto.Content }
                };
                 await DialogService.ShowDialogAsync<TopicDeatil>("帖子内容", parameters);
            }
        }
        protected async Task Best(object obj)
        {
            if (obj is BZTopicDtoWithUser dto)
            {
                await ConfirmAsync(async () =>
                {
                    await NetService.BestTopic(dto.Id);
                    MessageService.Show(dto.Good == 0 ? "加精成功" : "取消成功", MessageType.Success);
                    await SearchData();
                });
            }
        }
        protected async Task End(object obj)
        {
            if (obj is BZTopicDtoWithUser dto)
            {
                await ConfirmAsync(async () =>
                {
                    await NetService.EndTopic(dto.Id);
                    MessageService.Show("已结帖", MessageType.Success);
                    await SearchData();
                });
            }
        }
        
        protected async Task Del(object obj)
        {
            if (obj is BZTopicDtoWithUser dto)
            {
                await ConfirmAsync(async () =>
                {
                    await NetService.DelTopic(dto.Id,dto.Status.Value);
                    MessageService.Show("删除成功", MessageType.Success);
                    await SearchData();
                });
            }
        }
    }
}
