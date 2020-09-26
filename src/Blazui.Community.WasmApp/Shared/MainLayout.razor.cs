using Blazui.Community.WasmApp.Components;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Blazui.Community.WasmApp.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        protected BLayout Blayout { get; set; }
        protected AppHeader Appheader { get; set; }
        protected BCard Bcard { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                await base.OnAfterRenderAsync(firstRender);
                if (!firstRender)
                    return;

                Blayout.Refresh();
                Appheader.Refresh();
                Bcard.Refresh();
            }
            catch (Exception ex)
            {
                Console.WriteLine("OnAfterRenderAsync" + ex.Message);
            }
        }
    }
}