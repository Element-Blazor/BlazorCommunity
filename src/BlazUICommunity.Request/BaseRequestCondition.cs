using System;

namespace Blazui.Community.Request
{
    public class BaseRequestCondition
    {
        /// <summary>
        /// 创建时间开始
        /// </summary>
        [ExpressionQuery(OperationType.GreaterThanOrEqual, "CreateDate")]
        public DateTime? CreateDateStart { get; set; }

        /// <summary>
        /// 创建时间截止
        /// </summary>
        [ExpressionQuery(OperationType.LessThanOrEqual, "CreateDate")]
        public DateTime? CreateDateEnd { get; set; }

        /// <summary>
        /// 更新时间开始
        /// </summary>
        [ExpressionQuery(OperationType.GreaterThanOrEqual, "LastModifyDate")]
        public DateTime? LastModifyDateStart { get; set; }

        /// <summary>
        /// 更新时间截止
        /// </summary>
        [ExpressionQuery(OperationType.LessThanOrEqual, "LastModifyDate")]
        public DateTime? LastModifyDateEnd { get; set; }

        /// <summary>
        /// 状态 0 正常，-1 已删除
        /// </summary>
        [ExpressionQuery(OperationType.Equal)]
        public int? Status { get; set; } = null;

        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 15;
    }
}