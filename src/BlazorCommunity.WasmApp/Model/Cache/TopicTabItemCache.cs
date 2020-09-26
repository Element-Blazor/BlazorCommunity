using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorCommunity.DTO;

namespace BlazorCommunity.WasmApp.Model.Cache
{
    public class TopicTabItemCache:LocalStorgeCacheBase
    {
        public List<BZTopicDto> BzTopicDtos { get; set; }
        public int Total { get; set; }
    }
}
