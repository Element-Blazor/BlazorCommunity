using System;
using System.Collections.Generic;
using System.Text;

namespace BlazUICommunity.Utility.Request
{
   public abstract class BaseRequest
    {
        public PageInfo pageInfo { get; set; } = new PageInfo();
    }
}
