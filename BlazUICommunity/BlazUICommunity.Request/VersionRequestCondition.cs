using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Community.Request
{
    public class VersionRequestCondition: BaseRequestCondition
    {
        public int ProjectId { get; set; } = -1;
    }
}
