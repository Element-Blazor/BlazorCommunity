using Blazui.Community.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Admin.QueryCondition
{
    public class QueryReplyCondition 
    {

        /// <summary>
        /// 发布时间开始
        /// </summary>
        public DateTime? PublishTimeStart { get; set; }
        /// <summary>
        /// 发布时间结束
        /// </summary>
        public DateTime? PublishTimeEnd { get; set; }

        /// <summary>
        /// 修改时间开始
        /// </summary>
        public DateTime? ModifyTimeStart { get; set; }
        /// <summary>
        /// 修改时间结束
        /// </summary>
        public DateTime? ModifyTimeEnd { get; set; }

        /// <summary>
        /// 发帖人ID
        /// </summary>
        public object UserId { get; set; } 
        /// <summary>
        /// 状态 0正常，-1 删除，1已结帖
        /// </summary>
        public int? Status { get; set; } = 0;

        /// <summary>
        /// 回复的主题帖ID
        /// </summary>
        public object TopicId { get; set; }
    }
}
