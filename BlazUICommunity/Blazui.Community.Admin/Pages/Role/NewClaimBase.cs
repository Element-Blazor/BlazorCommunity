using Blazui.Community.Admin.Service;
using Blazui.Community.DTO.Admin;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Admin.Pages.Role
{
    public class NewClaimBase:BDialogBase
    {

        [Parameter]
        public string RoleId { get; set; }
        protected BForm claimForm;
        protected RoleClaimDto model;

        [Inject]
        private NetworkService NetService { get; set; }

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

            await CloseAsync(NewClaimResult.IsSuccess);
        }
    }
}
