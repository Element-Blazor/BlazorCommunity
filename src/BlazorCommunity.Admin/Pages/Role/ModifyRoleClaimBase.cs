using BlazorCommunity.Admin.Service;
using BlazorCommunity.DTO.Admin;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCommunity.Admin.Pages.Role
{
    public class ModifyRoleClaimBase : BDialogBase
    {

        internal BForm roleForm;

        [Parameter]
        public string RoleId { get; set; }

        protected BTable table;
        [Inject]
        protected  MessageBox MessageBox { get; set; }

        public List<RoleClaimDto> Claims { get; set; } = new List<RoleClaimDto>();
        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await LoadDatas();
            }
        }

        [Inject]
        private NetworkService NetService { get; set; }
        [Inject]
        public MessageService MessageService { get; set; }
      

        protected async Task New()
        {
            var model = new Dictionary<string, object>
            {
                { "RoleId", RoleId }
            };
            DialogResult result = await DialogService.ShowDialogAsync<NewClaim>("添加Claim", 500, model);
            if (Convert.ToBoolean(result.Result))
            {
                await LoadDatas();
            }
        }
        protected override bool ShouldRender() => true;
        private async Task LoadDatas()
        {
            var RoleClaimResponse = await NetService.QueryRoleClaims(RoleId);
            if (RoleClaimResponse.IsSuccess)
            {
                Claims = RoleClaimResponse.Data;
                table?.MarkAsRequireRender();
                RequireRender = true;
                this.StateHasChanged();
            }
        }

        protected async Task Delete(object context)
        {
            var claim = ((RoleClaimDto)context);
          
            MessageBoxResult Confirm = await MessageBox.ConfirmAsync("确定要删除该记录吗？");
            if (Confirm == MessageBoxResult.Ok)
            {
                var DeleteResult = await NetService.DeleteRoleClaim(claim);
                if (DeleteResult.IsSuccess)
                {
                    await LoadDatas();
                }
                MessageService.Show(DeleteResult.Message, DeleteResult.IsSuccess ? MessageType.Success : MessageType.Error);
            }
            else
                MessageService.Show("您选择了取消", MessageType.Info);
        }
    }
}
