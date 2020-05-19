using Blazui.Community.Admin.Service;
using Blazui.Community.DTO.Admin;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Admin.Pages.User
{
    public class RolesDialogBase:BDialogBase
    {

        internal BForm roleForm;

        [Parameter]
        public UserDisplayDto model { get; set; }

        public RolesDto rolesDto { get; set; }

        public Status Status { get; set; }
        public List<RoleDisplayDto> Values { get; set; } = new List<RoleDisplayDto>();
        public ObservableCollection<RoleDisplayDto> SelectedValues { get; set; } = new ObservableCollection<RoleDisplayDto>();
        [Inject]
        public NetworkService NetService { get; set; }
        [Inject]
        public MessageService MessageService { get; set; }
        protected  async override Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                var rolesResut = await NetService.QueryRoles(true);
                if (rolesResut != null)
                    Values = rolesResut.IsSuccess ? rolesResut.Data : new List<RoleDisplayDto>();

                var UserRolesResult = await NetService.QueryUserRoles(model.Id);

                if (UserRolesResult.IsSuccess)
                {
                    var userroles = UserRolesResult.Data;
                    SelectedValues = new ObservableCollection<RoleDisplayDto>(Values.Where(p => userroles.Contains(p.Name)));
                }
                else
                {
                    SelectedValues = new ObservableCollection<RoleDisplayDto>();
                }
                Status = Status.Indeterminate;
                RequireRender = true;
                StateHasChanged();
            }
          
        }

        protected override bool ShouldRender() => true;
        internal async Task Save()
        {

            var SetRoleResult = await NetService.SetUserRoles(new UserRoleDto { UserId=model.Id, RoleIds= SelectedValues.Select(p=>p.Id).ToList() });
            MessageService.Show(SetRoleResult.IsSuccess ? "Success" : SetRoleResult.Message, SetRoleResult.IsSuccess ? MessageType.Success : MessageType.Error);
            await CloseAsync(SetRoleResult.IsSuccess);
        }

        public void SelectAll(Status status)
        {
            Status = status;
            if (Status == Status.Checked)
            {
                SelectedValues = new ObservableCollection<RoleDisplayDto>(Values);
            }
            else if (Status == Status.UnChecked)
            {
                SelectedValues = new ObservableCollection<RoleDisplayDto>();
            }
        }

        public void ChangeStatus(Status status, RoleDisplayDto item)
        {
            if (status == Status.UnChecked)
            {
                SelectedValues.Remove(item);
            }
            else
            {
                SelectedValues.Add(item);
            }

            if (Values.All(SelectedValues.Contains))
            {
                Status = Status.Checked;
            }
            else if (SelectedValues.Any())
            {
                Status = Status.Indeterminate;
            }
            else
            {
                Status = Status.UnChecked;
            }
        }
    }
}
