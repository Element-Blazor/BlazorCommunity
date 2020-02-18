using Blazui.Community.Admin.QueryCondition;
using Blazui.Community.DTO;
using Blazui.Component;
using Blazui.Component.Form;
using Blazui.Component.Table;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Admin.Data
{
    public class UserManageBase : ManagePageBase
    {
        protected int pageSize = 6;
        protected int currentPage = 1;
        internal bool requireRender = false;
        protected List<BZUserUIDto> Datas = new List<BZUserUIDto>();
        protected int DataCount = 5;
        protected BTable table;
        protected BForm searchForm;
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
                SearchData();
            }
        }

        protected async Task SearchData()
        {
            await table.WithLoadingAsync(async () =>
            {
                await LoadDatas();
                UpdateUI();
            });
        }

        protected override async Task InitilizePageDataAsync()
        {
            await SearchData();
        }

        private async Task LoadDatas()
        {
            var condition = searchForm.GetValue<QueryUserCondition>();

            condition ??= new QueryUserCondition();
            var datas = await NetService.QueryUsers(condition);
            if (datas.IsSuccess)
            {
                Datas = datas.Data.Items.ToList();
                DataCount = datas.Data.TotalCount;
            }
            else
            {
                Datas = new List<BZUserUIDto>();
                DataCount = 0;
            }
        }

        private void UpdateUI()
        {
            requireRender = true;
            searchForm?.MarkAsRequireRender();
            table?.MarkAsRequireRender();
            table?.Refresh();
            StateHasChanged();
        }


        protected async Task Frozen(object obj)
        {
            if (obj is BZUserUIDto dto)
            {
                await ConfirmAsync(async () =>
                {
                    await NetService.FrozenUser(dto.Id);
                    MessageService.Show("冻结成功", MessageType.Success);
                    await SearchData();
                },
                "确定要冻结该账号吗？");
              
            }
        }
        protected async Task Detail(object obj)
        {

        }
        protected async Task UnFrozen(object obj)
        {
            if (obj is BZUserUIDto dto)
            {
                await ConfirmAsync(async () =>
                {
                    await NetService.UnFrozen(dto.Id);
                    MessageService.Show("激活成功", MessageType.Success);
                    await SearchData();
                },
                "确定要激活该账号吗？");
            }
        }
        
        protected async Task ResetPassword(object obj)
        {
            if (obj is BZUserUIDto dto)
            {
                await ConfirmAsync(async () =>
                {
                    var result=   await NetService.ResetPassword(dto.Id);
                    if(result.IsSuccess)
                    {
                        await MessageBox.AlertAsync($"重置密码成功，新密码为{result.Data?.ToString()}，请牢记");
                        //todo--发送短信至用户

                        await SearchData();
                    }
                },
                "确定要重置密码吗？");
            }
        }

    }
}
