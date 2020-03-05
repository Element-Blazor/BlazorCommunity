using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Enums
{
    /// <summary>
    /// 
    /// </summary>
    public enum DelStatus
    {
        [Description("正常")]
        Normal = 0,
        [Description("已删除")]
        Deleted = -1,
        [Description("已结帖")]
        Over = 1
    }
}
