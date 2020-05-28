using Blazui.Community.WasmApp.Service;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.WasmApp
{
    public partial class App:ComponentBase
    {
        [Inject]
        BrowerService browerService { get; set; }
        public bool IsMobile { get; set; } = false;
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                IsMobile = await browerService.IsMobile();
                
                StateHasChanged();
            }
        }
      
        protected override bool ShouldRender()
        {
            return true;
        }
    }
}
