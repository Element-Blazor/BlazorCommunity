using BlazorCommunity.Admin.Enum;
using BlazorCommunity.Admin.Service;
using BlazorCommunity.DTO.Admin;
using Element;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCommunity.Admin.Pages.Role
{
    public class NewRoleBase : BDialogBase
    {
        internal BForm roleForm;

        [Parameter]
        public RoleDisplayDto model { get; set; }

        [Parameter]
        public EntryOperation EntryOperation { get; set; }


        [Inject]
        private NetworkService NetService { get; set; }
        [Inject]
        private MessageService MessageService { get; set; }
        protected async Task Save()
        {
            if (!roleForm.IsValid())
                return;
            var role = roleForm.GetValue<RoleDisplayDto>();
            if(EntryOperation== EntryOperation.Add)
            {
                var NewRoleResult = await NetService.AddRoleAsync(new NewRoleDto { Name = role.Name });
                if (!NewRoleResult.IsSuccess)
                    MessageService.Show(NewRoleResult.Message, MessageType.Error);
                await CloseAsync(NewRoleResult.IsSuccess);
            }
            else
            {
                var UpdateRoleResult = await NetService.UpdateRoleAsync(role);
                if (!UpdateRoleResult.IsSuccess)
                    MessageService.Show(UpdateRoleResult.Message, MessageType.Error);
                await CloseAsync(UpdateRoleResult.IsSuccess);
            }
        }
    }
}
