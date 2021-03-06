﻿using BlazorCommunity.Admin.QueryCondition;
using BlazorCommunity.DTO.Admin;
using Element;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorCommunity.Admin.Pages.Reply
{
    public class ReplyManageBase : ManagePageBase<ReplyDisplayDto>
    {
        protected override async Task LoadDatas(bool MustRefresh = false)
        {
            var datas = await NetService.QueryReplys(BuildCondition<QueryReplyCondition>(), MustRefresh);
            if (datas.IsSuccess)
                SetData(datas.Data.Items, datas.Data.TotalCount);
            else if (datas.Code == 204)
                SetData();
        }

        protected async Task Resume(object obj)
        {
            if (obj is ReplyDisplayDto dto)
            {
                await ConfirmService.ConfirmAsync(
                    async () => await NetService.ResumeReply(dto.Id),
                    async () => await SearchData(true));
            }
        }

        protected async Task Delete(object obj)
        {
            if (obj is ReplyDisplayDto dto)
            {
                await ConfirmService.ConfirmAsync(
                    async () => await NetService.DeleteReply(dto.Id),
                    async () => await SearchData(true));
            }
        }

        protected async Task Detail(object obj)
        {
            if (obj is ReplyDisplayDto dto)
            {
                var Parameters = new Dictionary<string, object> { { "Content", dto.Content } };
                //if (!dto.Content.Contains("upload/Topic"))
                await DialogService.ShowDialogAsync<ReplyDetail>("回复内容", 800, Parameters);
                //else
                //    await DialogService.ShowDialogAsync<ReplyDetail>("回复内容", true, Parameters);
            }
        }
    }
}