﻿using System;

namespace BlazorCommunity.Request
{
    public class ReplyRequestCondition : BaseRequestCondition
    {
        /// <summary>
        /// 发布时间开始
        /// </summary>
        [ExpressionQuery(OperationType.GreaterThanOrEqual, "CreateDate")]
        public DateTime? PublishTimeStart { get; set; }

        /// <summary>
        /// 发布时间结束
        /// </summary>
        [ExpressionQuery(OperationType.LessThanOrEqual, "CreateDate")]
        public DateTime? PublishTimeEnd { get; set; }

        /// <summary>
        /// 修改时间开始
        /// </summary>
        [ExpressionQuery(OperationType.GreaterThanOrEqual, "LastModifyDate")]
        public DateTime? ModifyTimeStart { get; set; }

        /// <summary>
        /// 修改时间结束
        /// </summary>
        [ExpressionQuery(OperationType.LessThanOrEqual, "LastModifyDate")]
        public DateTime? ModifyTimeEnd { get; set; }

        /// <summary>
        /// 回复的主题帖ID
        /// </summary>
        [ExpressionQuery(OperationType.Equal)]
        public string TopicId { get; set; }

        public string UserName { get; set; }
        public string Title { get; set; }
    }
}