using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.DTO
{
    /// <summary>
    /// 
    /// </summary>
    public enum ModelStatus
    {
        [Description("正常")]
        Normal=0,
        [Description("已删除")]
        Deleted=-1
    }
}
