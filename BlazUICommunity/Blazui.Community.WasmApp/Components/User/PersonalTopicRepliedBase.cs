﻿using Blazui.Community.WasmApp.Components.User;
using Blazui.Community.WasmApp.Model.Condition;
using Blazui.Community.WasmApp.Pages;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using Blazui.Component;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazui.Community.WasmApp.Components
{
    public class PersonalTopicRepliedBase : PersonalTopicTableBase<PersonalReplyDisplayDto>
    {


        protected override async Task LoadDatas()
        {
            var model = searchForm.GetValue<SearchPersonalReplyCondition>();
            var result = await NetService.QueryPersonalReplys(User.Id, currentPage, pageSize, model.Title ??= "");
            SetData(result?.Data?.Items, result.Data.TotalCount);
        }

        protected async Task ShowContent(object topic)
        {
            if (topic is PersonalReplyDisplayDto replyDto)
            {
                var Parameters = new Dictionary<string, object> { { "Content", replyDto.Content } };
                await DialogService.ShowDialogAsync<PersonalReplyContent>("回复内容", 800, Parameters);
            }
        }

        /// <summary>
        /// 删除纪录
        /// </summary>
        /// <param name="topic"></param>
        protected async Task Del(object topic)
        {
            MessageBoxResult Confirm = await MessageBox.ConfirmAsync("确定要删除？");
            if (Confirm == MessageBoxResult.Ok)
                if (topic is PersonalReplyDisplayDto replyDto)
                {
                    var result = await NetService.DeleteRelpy(replyDto.Id);
                    if (result.IsSuccess)
                    {
                        await LoadDatas();
                        ToastSuccess("删除成功");
                    }
                }
        }

        protected void LinktoTopic(object topic)
        {
            if (topic is PersonalReplyDisplayDto topicModel)
                base.LinktoTopic(topicModel.TopicId);
        }


    }
}