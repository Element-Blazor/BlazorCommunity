using Blazui.Community.WasmApp.Service;
using Blazui.Community.DTO;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazui.Community.WasmApp.Components
{
    public class ContentBannerBase : BComponentBase
    {
        internal List<BzBannerDto> Banners { get; set; } = new List<BzBannerDto>();

        [Inject]
        public NetworkService NetService { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (!firstRender)
                return;
            var ResultData = await NetService.QueryBanners();

            if (ResultData.IsSuccess)
            {
                Banners = ResultData.Data ??= new List<BzBannerDto>();
                RequireRender = true;
                StateHasChanged();
            }
            else if (ResultData.Code == 204)
            {
                Banners = new List<BzBannerDto>();
                RequireRender = true;
                StateHasChanged();
            }
        }
    }
}