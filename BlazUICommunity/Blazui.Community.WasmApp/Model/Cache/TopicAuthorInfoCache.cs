using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazui.Community.DTO;

namespace Blazui.Community.WasmApp.Model.Cache
{
    public class TopicAuthorInfoCache:LocalStorgeCacheBase
    {
        public HotUserDto HotUser { get; set; }
    }
}
