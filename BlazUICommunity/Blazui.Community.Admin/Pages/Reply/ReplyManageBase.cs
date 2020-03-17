using Blazui.Community.Admin.Pages.Topic;
using Blazui.Community.Admin.QueryCondition;
using Blazui.Community.DTO;
using Blazui.Component;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Admin.Pages.Reply
{
    public class ReplyManageBase : ManagePageBase<BZReplyDto>
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
            if (obj is BZReplyDto dto)
            {
                await ConfirmService.ConfirmAsync(
                    async () => await NetService.ResumeReply(dto.Id),
                    async () => await SearchData(true));
            }
        }

        protected async Task Delete(object obj)
        {
            if (obj is BZReplyDto dto)
            {
                await ConfirmService.ConfirmAsync(
                    async () => await NetService.DeleteReply(dto.Id),
                    async () => await SearchData(true));
            }
        }

        protected async Task Detail(object obj)
        {
            if (obj is BZReplyDto dto)
            {
                await DialogService.ShowDialogAsync<TopicDeatil>("回贴内容", new Dictionary<string, object> { { "Content", dto.Content } });
            }
        }
    }
}