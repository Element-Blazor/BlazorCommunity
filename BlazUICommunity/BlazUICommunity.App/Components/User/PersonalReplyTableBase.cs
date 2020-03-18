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
        protected IList<PersonalReplyDisplayDto> Datas = new List<PersonalReplyDisplayDto>();
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
                 var result = await NetService.QueryPersonalReplys(User.Id, currentPage, pageSize, model.Title);
                 if (result.IsSuccess)
                 {
                     Datas = result.Data.Items;
                     DataCount = result.Data.TotalCount;
                 }
                 else
                 {
                     Datas = new List<PersonalReplyDisplayDto>();
                     DataCount = 0;
                 }
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