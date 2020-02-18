using Blazui.Community.Utility.Extensions;
using Blazui.Community.Utility.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Blazui.Community.Request
{
  public  class ReplyRequest : BaseRequest
    {
      

        /// <summary>
        /// 发布时间开始
        /// </summary>
        [ExpressionQuery(OperationType.GreaterThanOrEqual , "PublishTime")]
        public DateTime? PublishTimeStart { get; set; }
        /// <summary>
        /// 发布时间结束
        /// </summary>
        [ExpressionQuery(OperationType.LessThanOrEqual , "PublishTime")]
        public DateTime? PublishTimeEnd { get; set; }

        /// <summary>
        /// 修改时间开始
        /// </summary>
        [ExpressionQuery(OperationType.GreaterThanOrEqual , "ModifyTime")]
        public DateTime? ModifyTimeStart { get; set; }
        /// <summary>
        /// 修改时间结束
        /// </summary>
        [ExpressionQuery(OperationType.LessThanOrEqual , "ModifyTime")]
        public DateTime? ModifyTimeEnd { get; set; }

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
        /// 回复的主题帖ID
        /// </summary>
        [ExpressionQuery(OperationType.Equal)]
        public int? TopicId { get; set; }
    }
}
