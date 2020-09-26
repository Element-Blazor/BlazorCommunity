using Blazui.Community.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.WasmApp.Model.Cache
{
    public class CurrentUserCache:LocalStorgeCacheBase
    {
        public BZUserDto User { get; set; }
    }
}
