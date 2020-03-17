using Blazui.Community.Admin.Enum;
using Blazui.Community.Admin.Service;
using Blazui.Community.Admin.ViewModel;
using Blazui.Community.DTO.Admin;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Admin.Pages.Version
{
    public class ModifyVersionBase : BDialogBase
    {
        internal BForm versionForm;

        [Parameter]
        public VersionDisplayDto model { get; set; }
        [Parameter]
        public EntryOperation EntryOperation { get; set; }
        [Inject]
        public NetworkService NetService { get; set; }
        internal async Task Save()
        {
            if (!versionForm.IsValid())
                return;
            var version = versionForm.GetValue<VersionDisplayDto>();
            if(EntryOperation== EntryOperation.Add)
            {
                version.CreateDate = DateTime.Now;
                version.LastModifyDate = DateTime.Now;
                var newResult = await NetService.NewVersion(version);
                await CloseAsync(newResult.IsSuccess);
            }
            else
            {
                version.LastModifyDate = DateTime.Now;
                var newResult = await NetService.UpdateVersion(version);
                await CloseAsync(newResult.IsSuccess);
            }
        }
    }
}
