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
    public class NewClaimBase:BDialogBase
    {

        [Parameter]
        public string RoleId { get; set; }
        protected BForm claimForm;
        protected RoleClaimDto model;

        [Inject]
        private NetworkService NetService { get; set; }
        [Inject]
        private MessageService MessageService { get; set; }
        protected override void OnInitialized()
        {
            model = new RoleClaimDto { RoleId = RoleId };
        }

        protected async Task Save()
        {
            if (!claimForm.IsValid())
            {
                return;
            }

            var newmodel = claimForm.GetValue<RoleClaimDto>();

            var NewClaimResult = await NetService.AddRoleClaimAsync(newmodel);
                if (!NewClaimResult.IsSuccess)
                    MessageService.Show(NewClaimResult.Message, MessageType.Error);
            await CloseAsync(NewClaimResult.IsSuccess);
        }
    }
}
