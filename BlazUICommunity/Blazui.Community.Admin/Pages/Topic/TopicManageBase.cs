using Blazui.Community.Admin.Pages;
using Blazui.Community.Admin.Pages.Topic;
using Blazui.Community.Admin.QueryCondition;
using Blazui.Community.DTO;
using Blazui.Component;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Admin.Pages.Topic
{
    public class TopicManageBase : ManagePageBase
    {
        protected int pageSize = 10;
        protected int currentPage = 1;
        internal bool requireRender = false;
        protected List<BZTopicDto> Datas = new List<BZTopicDto>();
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
                Datas = new List<BZTopicDto>();
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
            condition.PageIndex = currentPage;
            condition.PageSize = pageSize;
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

            if (obj is BZTopicDto dto)
            {
                await ConfirmAsync(
                    async () => await NetService.TopTopic(dto.Id),
                    async () => await SearchData()
                    );
            }
        }
        protected async Task Detail(object obj)
        {
            if (obj is BZTopicDto dto)
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
            if (obj is BZTopicDto dto)
            {
                await ConfirmAsync(
                     async () => await NetService.BestTopic(dto.Id),
                     async () => await SearchData()
                     );
            }
        }
        protected async Task End(object obj)
        {
            if (obj is BZTopicDto dto)
            {
                await ConfirmAsync(
                    async () => await NetService.EndTopic(dto.Id),
                    async () => await SearchData()
                    );
            }
        }

        protected async Task Del(object obj)
        {
            if (obj is BZTopicDto dto)
            {
                await ConfirmAsync(
                    async () => await NetService.DelTopic(dto.Id, dto.Status),
                    async () => await SearchData()
                    );
            }
        }
    }
}
