﻿using Blazui.Community.Admin.Pages;
using Blazui.Community.Admin.Pages.Topic;
using Blazui.Community.Admin.QueryCondition;
using Blazui.Community.DTO;
using Blazui.Community.DTO.Admin;
using Blazui.Component;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Admin.Pages.Topic
{
    public class TopicManageBase : ManagePageBase<TopicDisplayDto>
    {

        protected override async Task LoadDatas(bool MustRefresh = false)
        {
            var datas = await NetService.QueryTopics(BuildCondition<QueryTopicCondition>(), MustRefresh);

            if (datas.IsSuccess)
                SetData(datas.Data.Items, datas.Data.TotalCount);
            else if (datas.Code == 204)
                SetData();

        }

        protected async Task Top(object obj)
        {

            if (obj is TopicDisplayDto dto)
            {
                await ConfirmService.ConfirmAsync(
                    async () => await NetService.TopTopic(dto.Id),
                    async () => await SearchData(true)
                    );
            }
        }
        protected async Task Detail(object obj)
        {
            if (obj is TopicDisplayDto dto)
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
            if (obj is TopicDisplayDto dto)
            {
                await ConfirmService.ConfirmAsync(
                     async () => await NetService.BestTopic(dto.Id),
                     async () => await SearchData(true)
                     );
            }
        }
        protected async Task End(object obj)
        {
            if (obj is TopicDisplayDto dto)
            {
                await ConfirmService.ConfirmAsync(
                    async () => await NetService.EndTopic(dto.Id),
                    async () => await SearchData(true)
                    );
            }
        }

        protected async Task Delete(object obj)
        {
            if (obj is TopicDisplayDto dto)
            {
                await ConfirmService.ConfirmAsync(
                    async () => await NetService.DelTopic(dto.Id),
                    async () => await SearchData(true)
                    );
            }
        }


        protected async Task Resume(object obj)
        {
            if (obj is TopicDisplayDto dto)
            {
                await ConfirmService.ConfirmAsync(
                    async () => await NetService.ResumeTopic(dto.Id),
                    async () => await SearchData(true)
                    );
            }
        }
    }
}
