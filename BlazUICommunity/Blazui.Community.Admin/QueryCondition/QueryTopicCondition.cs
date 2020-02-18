using Blazui.Community.DTO;
using Blazui.Community.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Admin.QueryCondition
{
    public class QueryTopicCondition 
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

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
        public object? UserId { get; set; } 
        /// <summary>
        /// 状态 0正常，-1 删除，1已结帖
        /// </summary>
        public ModelStatus? Status { get; set; } 

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
        public TopicType? TopicType { get; set; } = null;
    }
}
