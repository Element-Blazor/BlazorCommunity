﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorCommunity.DTO;

namespace BlazorCommunity.WasmApp.Model.Cache
{
    public class BannerCache:LocalStorgeCacheBase
    {
        public List<BzBannerDto> Banners  { get; set; }
    }
}
