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
        /// 发布时间开始
        /// </summary>
        public DateTime? CreateDateStart { get; set; }
        /// <summary>
        /// 发布时间结束
        /// </summary>
        public DateTime? CreateDateEnd { get; set; }


        /// <summary>
        /// 发帖人ID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 状态 0正常，-1 删除，1已结帖
        /// </summary>
        public DelStatus? Status { get; set; }

        /// <summary>
        /// 回复的主题帖ID
        /// </summary>
        public object TopicId { get; set; }
    }
}
