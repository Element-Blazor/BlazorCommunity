using Blazui.Community.Utility.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Model.Condition
{
    public class BaseSearchCondition
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        public PageInfo pageInfo { get; set; } = new PageInfo() { PageSize = 5, PageIndex = 1 };
    }
}
