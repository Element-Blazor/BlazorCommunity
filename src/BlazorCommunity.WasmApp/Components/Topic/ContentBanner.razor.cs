﻿using System;
using BlazorCommunity.WasmApp.Service;
using BlazorCommunity.DTO;
using Element;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BlazorCommunity.WasmApp.Model.Cache;

namespace BlazorCommunity.WasmApp.Components.Topic
{
    public partial class ContentBanner : ElementComponentBase
    {
        [Inject]
        ILocalStorageCacheService localStorage { get; set; }
        internal  List<BzBannerDto> Banners { get; set; } = new List<BzBannerDto>();

        [Inject]
        public NetworkService NetService { get; set; }

        protected int ActiveIndex = 0;

        protected override async Task OnInitializedAsync()
        {
            var bannerCache = await localStorage.CreateOrGetCache<BannerCache>("BannerCache", async () =>
            {
                var ResultData = await NetService.QueryBanners();
                if (ResultData.IsSuccess)
                {
                    return new BannerCache
                    {
                        Banners = ResultData.Data ??= new List<BzBannerDto>(),
                        Expire = DateTime.Now.AddDays(1)
                    };
                }
                else
                {
                    return new BannerCache
                    {
                        Banners = new List<BzBannerDto>()
                    };
                }

            });
            Banners = bannerCache.Banners;
        }

        protected override bool ShouldRender() => true;
        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    await base.OnAfterRenderAsync(firstRender);
        //    if (!firstRender)
        //        return;
           
           

        //    //if (Banners.Count > 0)
        //    //    while (true)
        //    //    {
        //    //        StateHasChanged();
        //    //        await Task.Delay(3000);
        //    //        if (ActiveIndex == Banners.Count - 1)
        //    //            ActiveIndex = 0;
        //    //        else
        //    //            ActiveIndex++;
        //    //    }
        //}
    }

   
}