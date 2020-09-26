using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorCommunity.DTO;

namespace BlazorCommunity.WasmApp.Model.Cache
{
    public class TopicAuthorInfoCache:LocalStorgeCacheBase
    {
        public HotUserDto HotUser { get; set; }
    }
}
