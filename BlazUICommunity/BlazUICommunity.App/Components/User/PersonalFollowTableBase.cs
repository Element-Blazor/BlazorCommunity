using Blazui.Community.App.Model.Condition;
using Blazui.Community.App.Pages;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using Blazui.Component;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Components
{
    public class PersonalFollowTableBase : PageBase
    {
        protected int pageSize = 6;
        protected int currentPage = 1;
        protected IList<PersonalFollowDisplayDto> Datas = new List<PersonalFollowDisplayDto>();
        protected int DataCount = 5;
        protected BTable table;
        protected BZUserModel User;

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

        protected override async Task InitilizePageDataAsync()
        {
            User = await GetUser();
            await SearchData();
        }

        protected async Task SearchData()
        {
            await table?.WithLoadingAsync(async () =>
            {
                await LoadDatas();
            });
        }

        /// <summary>
        /// 删除纪录
        /// </summary>
        /// <param name="topic"></param>
        public async Task Del(object topic)
        {
            MessageBoxResult Confirm = await MessageBox.ConfirmAsync("确定要取消收藏？");
            if (Confirm == MessageBoxResult.Ok)
                if (topic is PersonalFollowDisplayDto followModel)
                {
                    var result = await NetService.CancelFollow(followModel.FollowId);
                    if (result.IsSuccess)
                    {
                        await SearchData();
                        ToastWarning("取消收藏了");
                    }
                }
        }

        protected override bool ShouldRender() => true;

        /// <summary>
        /// 调用webapi接口获取数据，转换数据，加载界面
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        protected async Task LoadDatas()
        {
            var result = await NetService.QueryFollows(CreateCondition());
            if (result == null)
                return;

            if (result.IsSuccess && result.Data != null && result.Data.Items.Any())
            {
                Datas = result.Data.Items;
                DataCount = result.Data.TotalCount;
            }
            else
            {
                Datas = new List<PersonalFollowDisplayDto>();
                DataCount = 0;
            }

            UpdateUI();
        }

        private SearchPersonalFollowCondition CreateCondition()
        {
            var condition = searchForm.GetValue<SearchPersonalFollowCondition>();
            condition ??= new SearchPersonalFollowCondition();
            condition.CreatorId = User.Id;
            condition.PageSize = pageSize;
            condition.PageIndex = currentPage;
            return condition;
        }

        private void UpdateUI()
        {
            table?.MarkAsRequireRender();
            table?.Refresh();
            StateHasChanged();
        }

        protected void LinktoTopic(object topic)
        {
            if (topic is PersonalFollowDisplayDto topicModel)
                navigationManager.NavigateTo($"/topic/{topicModel.Id}");
        }
    }
}