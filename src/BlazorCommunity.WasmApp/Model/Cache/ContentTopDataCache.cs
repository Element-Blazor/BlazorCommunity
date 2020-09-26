using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorCommunity.DTO;

namespace BlazorCommunity.WasmApp.Model.Cache
{
    public class ContentTopDataCache:LocalStorgeCacheBase
    {
        public List<BZTopicDto> Topics   { get; set; }
    }
}
