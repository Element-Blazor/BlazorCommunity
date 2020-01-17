using BlazUICommunity.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazUICommunity.Utility.Request
{
   public class PageInfo
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
