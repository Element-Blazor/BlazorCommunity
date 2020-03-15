﻿using Blazui.Community.Admin.QueryCondition;
using Blazui.Community.DTO;
using Blazui.Community.DTO.Admin;
using Blazui.Component;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Admin.Pages.User
{
    public class UserManageBase : ManagePageBase
    {
        protected int pageSize = 10;
        protected int currentPage = 1;
        internal bool requireRender = false;
        protected IList<UserDisplayDto> Datas = new List<UserDisplayDto>();
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
            condition.PageIndex = currentPage;
            condition.PageSize = pageSize;
            var datas = await NetService.QueryUsers(condition);
            //是否可以做一个判断 如果返回的相同数据，就重新渲染了??
            if (datas.IsSuccess)
            {
                Datas = datas.Data.Items;
                DataCount = datas.Data.TotalCount;
                UpdateUI();
            }
            else
            {
                if (datas.Code != 304)
                {
                    Datas = new List<UserDisplayDto>();
                    DataCount = 0;
                    UpdateUI();
                }
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
            if (obj is UserDisplayDto dto)
            {
                await ConfirmAsync(
                    async () => await NetService.FrozenUser(dto.Id),
                     async () => await SearchData(),
                "确定要冻结该账号吗？");

            }
        }
        protected async Task Detail(object obj)
        {
            MessageService.Show(((UserDisplayDto)obj).UserName);
        }
        protected async Task UnFrozen(object obj)
        {
            if (obj is UserDisplayDto dto)
            {
                await ConfirmAsync(
                    async () => await NetService.UnFrozen(dto.Id),
                    async () => await SearchData(),
                "确定要激活该账号吗？");
            }
        }

        protected async Task ResetPassword(object obj)
        {
            if (obj is UserDisplayDto dto)
            {
                await ConfirmAsync(
                    async () => await NetService.ResetPassword(dto.Id),
                    async result =>
                    {
                        await MessageBox.AlertAsync($"重置密码成功，新密码为{result.Data?.ToString()}，请牢记");
                        //todo--发送短信至用户
                        await SearchData();
                    },

                "确定要重置密码吗？");
            }
        }

    }
}
