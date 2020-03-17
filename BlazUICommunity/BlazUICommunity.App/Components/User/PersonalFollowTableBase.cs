using Blazui.Community.App.Model;
using Blazui.Community.App.Model.Condition;
using Blazui.Community.App.Pages;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using Blazui.Community.Response;
using Blazui.Component;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazui.Community.App.Components
{
    public class PersonalFollowTableBase : PageBase
    {
        protected int pageSize = 6;
        protected int currentPage = 1;
        internal bool requireRender = false;
        protected List<PersonalTopicModel> Datas = new List<PersonalTopicModel>();
        protected int DataCount = 5;
        protected BTable table;
        protected BZUserModel User;

        protected BForm searchForm;

        private SearchPersonalFollowCondition Condition => new SearchPersonalFollowCondition
        {
            PageIndex = currentPage,
            PageSize = pageSize
        };

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

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (!firstRender)
                return;
            await LoadDatas();
        }

        private async Task LoadDatas(SearchPersonalFollowCondition condition = null)
        {
            await table?.WithLoadingAsync(async () =>
            {
                await QueryFollows(condition);
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
                if (topic is PersonalTopicModel followModel)
                {
                    var result = await NetService.DelFollows(followModel.Id, User.Id);
                    if (result.IsSuccess)
                    {
                        await LoadDatas();
                        ToastWarning("取消收藏了");
                    }
                }
        }

        protected override bool ShouldRender() => requireRender;

        /// <summary>
        /// 调用webapi接口获取数据，转换数据，加载界面
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        protected async Task QueryFollows(SearchPersonalFollowCondition condition = null)
        {
            User = await GetUser();
            condition = CreateCondition(condition);
            var result = await NetService.GetFollows(condition);
            if (result == null)
                return;
            ConvertDataToDto(result);
            UpdateUI();
        }

        private SearchPersonalFollowCondition CreateCondition(SearchPersonalFollowCondition condition)
        {
            condition = searchForm.GetValue<SearchPersonalFollowCondition>();
            condition ??= new SearchPersonalFollowCondition();
            condition.CreatorId = User.Id;
            condition.PageSize = pageSize;
            condition.PageIndex = currentPage;
            return condition;
        }

        private void ConvertDataToDto(BaseResponse<PageDatas<BZTopicDto>> result)
        {
            if (result.IsSuccess && result.Data != null && result.Data.TotalCount > 0)
            {
                Datas = mapper.Map<List<PersonalTopicModel>>(result.Data.Items);
                DataCount = result.Data.TotalCount;
            }
            else
            {
                Datas = new List<PersonalTopicModel>();
                DataCount = 0;
            }
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <returns></returns>
        protected async Task SearchData() => await LoadDatas(Condition);

        private void UpdateUI()
        {
            requireRender = true;
            table?.MarkAsRequireRender();
            table?.Refresh();
            StateHasChanged();
        }

        protected void LinktoTopic(object topic)
        {
            if (topic is PersonalTopicModel topicModel)
                navigationManager.NavigateTo($"/topic/{topicModel.Id}");
        }
    }
}