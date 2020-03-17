using Blazui.Community.App.Service;
using Blazui.Community.DTO;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazui.Community.App.Components
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
            var ResultData = await NetService.GetBanners();
            if (ResultData.IsSuccess)
            {
                Banners = ResultData.Data;
                RequireRender = true;
                StateHasChanged();
            }
        }
    }
}