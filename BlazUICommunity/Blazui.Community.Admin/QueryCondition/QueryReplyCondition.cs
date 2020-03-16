using Blazui.Community.DTO;
using Blazui.Community.Enums;
using Blazui.Community.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Admin.QueryCondition
{
    public class QueryReplyCondition : QueryBaseCondition
    {


        /// <summary>
        /// 发帖人ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 回复的主题帖ID
        /// </summary>
        public object TopicId { get; set; }

        public string UserName { get; set; }

        public string Title { get; set; }
    }
}
