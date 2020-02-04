using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Community.Utility.Request
{
   public abstract class BaseRequest
    {
        public PageInfo pageInfo { get; set; } = new PageInfo();
    }
}
