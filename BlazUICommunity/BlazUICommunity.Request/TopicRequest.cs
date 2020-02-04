using Blazui.Community.Utility.Extensions;
using Blazui.Community.Utility.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Blazui.Community.Request
{
  public  class TopicRequest : BaseRequest
    {
        /// <summary>
        /// 标题
        /// </summary>
        [ExpressionQuery(OperationType.Like)]
        public string Title { get; set; }

        /// <summary>
        /// 发布时间开始
        /// </summary>
        [ExpressionQuery(OperationType.GreaterThanOrEqual, "PublishTime")]
        public DateTime PublishTimeStart { get; set; }
        /// <summary>
        /// 发布时间结束
        /// </summary>
        [ExpressionQuery(OperationType.LessThanOrEqual , "PublishTime")]
        public DateTime PublishTimeEnd { get; set; }

        /// <summary>
        /// 修改时间开始
        /// </summary>
        [ExpressionQuery(OperationType.GreaterThanOrEqual , "ModifyTime")]
        public DateTime ModifyTimeStart { get; set; }
        /// <summary>
        /// 修改时间结束
        /// </summary>
        [ExpressionQuery(OperationType.LessThanOrEqual , "ModifyTime")]
        public DateTime ModifyTimeEnd { get; set; }

        /// <summary>
        /// 发帖人ID
        /// </summary>
        [ExpressionQuery(OperationType.Equal)]
        public int? UserId { get; set; } = null;
        /// <summary>
        /// 状态 0正常，-1 删除，1已结帖
        /// </summary>
        [ExpressionQuery(OperationType.Equal)]
        public int? Status { get; set; } = 0;

        /// <summary>
        /// 是否置顶0否-1置顶
        /// </summary>
        [ExpressionQuery(OperationType.Equal)]
        public int? Top { get; set; } = null;
        /// <summary>
        /// 是否精华帖0否，1-是
        /// </summary>  
        [ExpressionQuery(OperationType.Equal)]
        public int? Good { get; set; } = null;
        /// <summary>
        /// 主题帖类型 0：提问，1：分享，2：讨论，3：建议，4：公告
        /// </summary>
        [ExpressionQuery(OperationType.Equal)]
        public int? TopicType { get; set; } = null;


    }
}
