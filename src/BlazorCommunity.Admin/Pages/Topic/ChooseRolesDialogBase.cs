using BlazorCommunity.Admin.Service;
using BlazorCommunity.DTO.Admin;
using Element;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCommunity.Admin.Pages.Topic
{
    public class ChooseRolesDialogBase:BDialogBase
    {

        [Parameter]
        public string RoleId { get; set; }
        [Parameter]
        public string TopicId { get; set; }

        public List<RoleDisplayDto> Values { get; set; } = new List<RoleDisplayDto>();
        public string SelectedValue { get; set; } 
        [Inject]
        public NetworkService NetService { get; set; }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                var rolesResut = await NetService.QueryRoles(true);
                if (rolesResut != null)
                {
                    Values = rolesResut.IsSuccess ? rolesResut.Data : new List<RoleDisplayDto>();
                    Values.Insert(0, new RoleDisplayDto { Id = "-", Name = "无权限" });
                    SelectedValue = Values.FirstOrDefault(p => p.Id == RoleId)?.Id;
                    RequireRender = true;
                    StateHasChanged();
                }
            }
        }

        protected override bool ShouldRender() => true;
        internal async Task Save()
        {
            var SetRoleResult = await NetService.SetAuthorizeToTopic(TopicId, SelectedValue);
            await CloseAsync(SetRoleResult.IsSuccess);
        }
    }
}
