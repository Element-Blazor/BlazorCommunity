using BlazorCommunity.Admin.QueryCondition;
using BlazorCommunity.Admin.Service;
using BlazorCommunity.DictionaryExtensions;
using BlazorCommunity.DTO.Admin;
using Element;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorCommunity.Admin.Pages.User
{
    public class UserManageBase : ManagePageBase<UserDisplayDto>
    {
        [Inject]
        public NetworkService test { get; set; }
        protected override async Task LoadDatas(bool MustRefresh = false)
        {
            //test.Show();
            var datas = await test.QueryUsers(BuildCondition<QueryUserCondition>(), MustRefresh);
            //是否可以做一个判断 如果返回的相同数据，就重新渲染了??
            if (datas.IsSuccess)
                SetData(datas.Data.Items, datas.Data.TotalCount);
            else if (datas.Code == 204)
                SetData();
        }

        protected async Task Delete(object obj)
        {
            if (obj is UserDisplayDto dto)
            {
                await ConfirmService.ConfirmAsync(
                    async () => await NetService.DeleteUser(dto.Id),
                     async () => await SearchData(true),
                "确定要冻结该账号吗？");
            }
        }

        protected async Task Detail(object obj)
        {
            MessageService.Show(((UserDisplayDto)obj).UserName);
            await Task.CompletedTask;
        }


        protected async Task SetRole(object context)
        {
            if (context is UserDisplayDto dto)
            {
                var model = dto.ObjectToDictionary("model");
                DialogResult result = await DialogService.ShowDialogAsync<RolesDialog>("设置用户角色", 700, model);
                if (Convert.ToBoolean(result.Result))
                {
                    await SearchData(true);
                }
            }
        }


        protected async Task Resume(object obj)
        {
            if (obj is UserDisplayDto dto)
            {
                await ConfirmService.ConfirmAsync(
                    async () => await NetService.ResumeUser(dto.Id),
                    async () => await SearchData(true),
                "确定要激活该账号吗？");
            }
        }

        protected async Task ResetPassword(object obj)
        {
            if (obj is UserDisplayDto dto)
            {
                await ConfirmService.ConfirmAsync(
                    async () => await NetService.ResetPassword(dto.Id),
                    async result =>
                    {
                        await MessageBox.AlertAsync($"重置密码成功，新密码为{result.Data?.ToString()}，请牢记");
                        //todo--发送短信至用户
                        await SearchData(true);
                    },

                "确定要重置密码吗？");
            }
        }
    }
}