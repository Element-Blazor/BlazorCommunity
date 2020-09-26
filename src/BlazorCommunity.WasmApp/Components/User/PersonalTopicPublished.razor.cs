using BlazorCommunity.WasmApp.Model.Condition;
using BlazorCommunity.WasmApp.Pages;
using BlazorCommunity.DTO;
using BlazorCommunity.Enums;
using BlazorCommunity.Model.Models;
using Blazui.Component;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorCommunity.WasmApp.Components.User;

namespace BlazorCommunity.WasmApp.Components.User
{
    [Authorize]
    public partial class PersonalTopicPublished : PersonalTopicTableBase<PersonalTopicDisplayDto>
    {
        protected TopicCategory? Category;
        protected override async Task LoadDatas()
        {
            var result = await NetService.QueryPersonalTopics(CreateCondition());
            if (result != null)
                SetData(result.Data?.Items, result.Data?.TotalCount);
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
                base.LinktoTopic(topicModel.Id);
        }



        private SearchPersonalTopicCondition CreateCondition()
        {
            var condition = searchForm.GetValue<SearchPersonalTopicCondition>();
            condition ??= new SearchPersonalTopicCondition();
            condition.PageIndex = currentPage;
            condition.PageSize = pageSize;
            condition.CreatorId = User.Id;
            return condition;
        }

    }
}