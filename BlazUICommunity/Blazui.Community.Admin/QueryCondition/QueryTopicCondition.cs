using Blazui.Community.DTO;
using Blazui.Community.Enums;
using Blazui.Community.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Admin.QueryCondition
{
    public class QueryTopicCondition : BaseQueryCondition
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 是否置顶0否-1置顶
        /// </summary>
        public int? Top { get; set; } = null;
        /// <summary>
        /// 是否精华帖0否，1-是
        /// </summary>  
        public int? Good { get; set; } = null;
        /// <summary>
        /// 主题帖类型 0：提问，1：分享，2：讨论，3：建议，4：公告
        /// </summary>
        public TopicCategory? Category { get; set; } 

        public string UserName { get; set; }
    }
}
