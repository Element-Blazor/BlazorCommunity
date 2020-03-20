using Blazui.Community.Admin.Pages.Topic;
using Blazui.Community.Admin.QueryCondition;
using Blazui.Community.DTO;
using Blazui.Community.DTO.Admin;
using Blazui.Component;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Admin.Pages.Reply
{
    public class ReplyManageBase : ManagePageBase<ReplyDisplayDto>
    {
        protected override async Task LoadDatas(bool MustRefresh = false)
        {
            var datas = await NetService.QueryReplys(BuildCondition<QueryReplyCondition>(), MustRefresh);
            if (datas.IsSuccess)
            {
                datas.Data.Items.Where(p => p.Content.Length > 60).ToList().ForEach(p =>
                {
                    p.Content = p.Content.Substring(0, 60) + "...";
                });
                SetData(datas.Data.Items, datas.Data.TotalCount);
            }
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
                if (dto.Content.Length < 100)
                    await DialogService.ShowDialogAsync<ReplyDetail>("回复内容", 500, Parameters);
                else if (dto.Content.Length < 500)
                    await DialogService.ShowDialogAsync<ReplyDetail>("回复内容", 800, Parameters);
                else
                    await DialogService.ShowDialogAsync<ReplyDetail>("回复内容", true, Parameters);
            }
        }
    }
}