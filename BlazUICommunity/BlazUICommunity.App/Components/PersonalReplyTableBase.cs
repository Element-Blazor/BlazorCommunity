using Blazui.Community.App.Pages;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using Blazui.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Components
{
    public class PersonalReplyTableBase : PageBase
    {

        protected int pageSize = 6;
        protected int currentPage = 1;
        internal bool requireRender = false;
        protected List<ReplyDto> Datas = new List<ReplyDto>();
        protected int DataCount = 5;
        protected BTable table;
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

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (!firstRender)
                return;
            await LoadDatas();
        }

        private async Task LoadDatas()
        {
            await table.WithLoadingAsync(async () =>
            {
                var result = await NetService.GetMyReplys((await GetUser()).Id, currentPage, pageSize);
                if (result.IsSuccess)
                {
                    Datas = result.Data;
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
                if (topic is ReplyDto  replyDto)
                {
                    var result = await NetService.DelRelpy(replyDto.Id);
                    if (result.IsSuccess)
                    {
                        await LoadDatas();
                        MessageService.Show($"删除成功", MessageType.Success);
                    }
                }
        }

        protected override bool ShouldRender()
        {
            return requireRender;
        }

        protected override async Task InitilizePageDataAsync()
        {
            await Task.CompletedTask;
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
