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
        internal static List<BzBannerDto> Banners { get; set; } = new List<BzBannerDto>();

        [Inject]
        public NetworkService NetService { get; set; }

        protected int ActiveIndex=0;

        protected override bool ShouldRender() => true;
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (!firstRender)
                return;
            if(Banners.Count==0)
            {
                var ResultData = await NetService.QueryBanners();
                if (ResultData.IsSuccess)
                {
                    Banners = ResultData.Data ??= new List<BzBannerDto>();
                }
                else if (ResultData.Code == 204)
                {
                    Banners = new List<BzBannerDto>();
                    RequireRender = true;
                }
            }

            if(Banners.Count>0)
            while (true)
            {
                StateHasChanged();
                await Task.Delay(3000);
                if (ActiveIndex == Banners.Count - 1)
                    ActiveIndex = 0;
                else
                    ActiveIndex++;
            }
        }
    }
}