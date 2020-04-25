﻿using Blazui.Community.Admin.Enum;
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
    public class NewRoleBase : BDialogBase
    {
        internal BForm roleForm;

        [Parameter]
        public RoleDisplayDto model { get; set; }

        [Parameter]
        public EntryOperation EntryOperation { get; set; }


        [Inject]
        private NetworkService NetService { get; set; }

        protected async Task Save()
        {
            if (!roleForm.IsValid())
                return;
            var role = roleForm.GetValue<RoleDisplayDto>();
            if(EntryOperation== EntryOperation.Add)
            {
                var NewRoleResult = await NetService.AddRoleAsync(new NewRoleDto { Name = role.Name });
                await CloseAsync(NewRoleResult.IsSuccess);
            }
            else
            {
                var UpdateRoleResult = await NetService.UpdateRoleAsync(role);
                await CloseAsync(UpdateRoleResult.IsSuccess);
            }
        }
    }
}