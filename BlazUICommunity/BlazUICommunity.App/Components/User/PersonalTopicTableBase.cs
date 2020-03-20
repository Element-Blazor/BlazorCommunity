using Blazui.Community.App.Model.Condition;
using Blazui.Community.App.Pages;
using Blazui.Community.DTO;
using Blazui.Community.Enums;
using Blazui.Community.Model.Models;
using Blazui.Component;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazui.Community.App.Components
{
    [Authorize]
    public class PersonalTopicTableBase : PageBase
    {
        protected int pageSize = 6;
        protected int currentPage = 1;
        protected TopicCategory? Category;
        internal bool requireRender = false;
        protected  IList<PersonalTopicDisplayDto> Datas = new List<PersonalTopicDisplayDto>();
        protected int DataCount = 5;
        protected BTable table;
        protected BForm searchForm;
        protected BZUserModel User;

        private SearchPersonalTopicCondition Condition
        {
            get
            {
                var condition = searchForm.GetValue<SearchPersonalTopicCondition>();
                condition.PageIndex = currentPage;
                condition.PageSize = pageSize;
                return condition;
            }
        }

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

        private async Task LoadDatas(SearchPersonalTopicCondition condition = null)
        {
            if (table == null)
            {
                Console.WriteLine("table is null");
            }
            await table?.WithLoadingAsync(async () =>
            {
                await QueryTopics(condition);
            });
        }

        /// <summary>
        /// 删除纪录
        /// </summary>
        /// <param name="topic"></param>
        public async Task Del(object topic)
        {
            MessageBoxResult Confirm = await MessageBox.ConfirmAsync("确定要删除？");
            if (Confirm == MessageBoxResult.Ok)
                if (topic is PersonalTopicDisplayDto topicModel)
                {
                    var result = await NetService.DeleteTopic(topicModel.Id);
                    if (result.IsSuccess)
                    {
                        await LoadDatas();
                        ToastSuccess("删除成功");
                    }
                }
        }

        protected void LinktoTopic(object topic)
        {
            if (topic is PersonalTopicDisplayDto topicModel)
                navigationManager.NavigateTo($"/topic/{topicModel.Id}");
        }

        protected override bool ShouldRender() => true;

        /// <summary>
        /// 调用webapi接口获取数据，转换数据，加载界面
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        protected async Task QueryTopics(SearchPersonalTopicCondition condition = null)
        {
            User = await GetUser();
            condition = CreateCondition(condition);
            var result = await NetService.QueryPersonalTopics(condition);
            if (result.IsSuccess && result.Data != null && result.Data.TotalCount > 0)
            {
                Datas = result.Data.Items;
                DataCount = result.Data.TotalCount;
                UpdateUI();
            }
            else
            {
                if (result.Code == 204)
                {
                    Datas = new List<PersonalTopicDisplayDto>();
                    DataCount = 0;
                    UpdateUI();
                }
            }
        }

        private SearchPersonalTopicCondition CreateCondition(SearchPersonalTopicCondition condition)
        {
            condition ??= new SearchPersonalTopicCondition();
            condition.CreatorId = User.Id;
            condition.PageIndex = currentPage;
            condition.PageSize = pageSize;
            return condition;
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <returns></returns>
        protected async Task SearchData()
        {
            await LoadDatas(Condition);
        }

        private void UpdateUI()
        {
            requireRender = true;
            searchForm?.MarkAsRequireRender();
            table?.MarkAsRequireRender();
            table?.Refresh();
            StateHasChanged();
        }
    }
}