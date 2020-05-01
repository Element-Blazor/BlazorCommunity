﻿using Blazui.Community.WasmApp.Model.Condition;
using Blazui.Community.WasmApp.Pages;
using Blazui.Community.DTO;
using Blazui.Community.Enums;
using Blazui.Community.Model.Models;
using Blazui.Component;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazui.Community.WasmApp.Components.User;

namespace Blazui.Community.WasmApp.Components
{
    [Authorize]
    public class PersonalTopicPublishedBase : PersonalTopicTableBase<PersonalTopicDisplayDto>
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