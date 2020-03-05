using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Community.Request
{
  public  class BannerRequestCondition: BaseRequestCondition
    {
        [ExpressionQuery(OperationType.Like)]
        public string Title { get; set; } = null;
    }
}
