using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazui.Community.DTO;
using Blazui.Community.Response;

namespace Blazui.Community.WasmApp.Model.Cache
{
    public class HotTopicCache:LocalStorgeCacheBase
    {
        public List<HotTopicDto> List { get; set; }

    }
}
