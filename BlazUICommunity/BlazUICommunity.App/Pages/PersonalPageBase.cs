using Blazui.Component;
using Blazui.Component.Container;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Pages
{
    public abstract class PersonalPageBase : PageBase
    {
        protected BTab bTab { get; set; }
        protected string tabTitle { get; set; }
        protected BTabPanel bTabPanel { get; set; }

        protected abstract void InitTabTitle();

      
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {

                bTab?.Refresh();
                bTabPanel?.Refresh();
               this. StateHasChanged();
            }
            else
            {
                return;
            }

        }
        protected override bool ShouldRender()
        {
            return true;
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            InitTabTitle();
        }

        public async Task navigationToUpdateUserUI(string Uri)
        {
            LoadingService.Show(new LoadingOption()
            {
                Background = "rgba(0, 0, 0, 0.1)",
                Text = "",
                IconClass = "el-icon-loading"
            });
            await Task.Delay(500);
            MessageService.Show("更新成功", MessageType.Success);
            LoadingService.CloseFullScreenLoading();
            navigationManager.NavigateTo(Uri, true);
        }
    }
}
