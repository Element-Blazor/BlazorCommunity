using Blazui.Community.App.Model.Condition;
using Blazui.Community.App.Pages;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using Blazui.Component;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazui.Community.App.Components
{
    public class PersonalReplyTableBase : PageBase
    {
        protected int pageSize = 6;
        protected int currentPage = 1;
        internal bool requireRender = false;
        protected List<BZReplyDto> Datas = new List<BZReplyDto>();
        protected int DataCount = 5;
        protected BTable table;
        protected BForm searchForm;
        protected BZUserModel User { get; set; }

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
                LoadDatas();
            }
        }

        protected async Task SearchData()
        {
            await LoadDatas();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (!firstRender)
                return;
            await LoadDatas();
        }

        private async Task LoadDatas()
        {
            var model = searchForm.GetValue<SearchPersonalTopicCondition>();
            model.Title ??= "";
            await table?.WithLoadingAsync(async () =>
             {
                 DataCount = (int)(await NetService.GetMyReplysCount(User.Id, model.Title))?.Data;
                 if (DataCount > 0)
                 {
                     var result = await NetService.GetMyReplys(User.Id, currentPage, pageSize, model.Title);
                     if (result.IsSuccess)
                         Datas = result.Data;
                 }
                 else
                     Datas = new List<BZReplyDto>();

                 UpdateUI();
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
                if (topic is BZReplyDto replyDto)
                {
                    var result = await NetService.DelRelpy(replyDto.Id);
                    if (result.IsSuccess)
                    {
                        await LoadDatas();
                        ToastSuccess("删除成功");
                    }
                }
        }

        protected void LinktoTopic(object topic)
        {
            if (topic is BZReplyDto topicModel)
                navigationManager.NavigateTo($"/topic/{topicModel.TopicId}");
        }

        protected override bool ShouldRender() => true;

        protected override async Task InitilizePageDataAsync()
        {
            User = await GetUser();
        }

        private void UpdateUI()
        {
            requireRender = true;
            table?.MarkAsRequireRender();
            table?.Refresh();
            StateHasChanged();
        }
    }
}