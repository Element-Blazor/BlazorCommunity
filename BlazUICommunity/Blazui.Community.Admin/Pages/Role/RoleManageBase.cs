using Blazui.Community.Admin.Enum;
using Blazui.Community.DictionaryExtensions;
using Blazui.Community.DTO.Admin;
using Blazui.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Admin.Pages.Role
{
    public class RoleManageBase : ManagePageBase<RoleDisplayDto>
    {
        protected async override Task LoadDatas(bool MustRefresh = false)
        {
            var datas = await NetService.QueryRoles(MustRefresh);
            if (datas.IsSuccess)
                SetData(datas.Data.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList(), datas.Data.Count);
            else if (datas.Code == 204)
                SetData();
        }

        protected async Task Delete(object context)
        {
            await ConfirmService.ConfirmAsync(
                     async () => await NetService.DeleteRoleAsync(((RoleDisplayDto)context).Id),
                      async () => await SearchData(true),
                 "确定要删除该记录吗？");
        }

        protected async Task Modify(object context)
        {

            var model = ((RoleDisplayDto)context).ObjectToDictionary("model");
            model.Add("EntryOperation", EntryOperation.Update);
            DialogResult result = await DialogService.ShowDialogAsync<NewRole>("修改角色", 700, model);
            if (Convert.ToBoolean(result.Result))
                await SearchData(true);
        }

        protected async Task ModifyClaims(object context)
        {
            var model = new Dictionary<string, object>
            {
                { "RoleId", ((RoleDisplayDto)context).Id }
            };

            DialogResult result = await DialogService.ShowDialogAsync<ModifyRoleClaim>("设置Claims", 900, model);
            if (Convert.ToBoolean(result.Result))
                await SearchData(true);
        }
        protected async Task New()
        {
            var model = new Dictionary<string, object>
            {
                { "EntryOperation", EntryOperation.Add }
            };
            DialogResult result = await DialogService.ShowDialogAsync<NewRole>("添加角色", 700, model);
            if (Convert.ToBoolean(result.Result))
                await SearchData(true);
        }
    }
}
