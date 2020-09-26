using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorCommunity.DTO;
using BlazorCommunity.Response;

namespace BlazorCommunity.WasmApp.Model.Cache
{
    public class HotTopicCache:LocalStorgeCacheBase
    {
        public List<HotTopicDto> List { get; set; }

    }
}
