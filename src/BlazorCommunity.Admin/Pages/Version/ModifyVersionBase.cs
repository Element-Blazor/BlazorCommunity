using BlazorCommunity.Admin.Enum;
using BlazorCommunity.Admin.Service;
using BlazorCommunity.DTO.Admin;
using Element;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace BlazorCommunity.Admin.Pages.Version
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
        [Inject]
        private MessageService MessageService { get; set; }
        internal async Task Save()
        {
            if (!versionForm.IsValid())
                return;
            var version = versionForm.GetValue<VersionDisplayDto>();
            version.VerName = version.VerNo;
            version.LastModifyDate = DateTime.Now;
            if (EntryOperation == EntryOperation.Add)
            {
                version.CreateDate = DateTime.Now;
                var newResult = await NetService.NewVersion(version);
                if (!newResult.IsSuccess)
                    MessageService.Show(newResult.Message, MessageType.Error);
                await CloseAsync(newResult.IsSuccess);
            }
            else
            {
                var newResult = await NetService.UpdateVersion(version);
                if (!newResult.IsSuccess)
                    MessageService.Show(newResult.Message, MessageType.Error);
                await CloseAsync(newResult.IsSuccess);
            }
        }
    }
}