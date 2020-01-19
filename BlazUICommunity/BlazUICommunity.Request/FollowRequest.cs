using BlazUICommunity.Utility.Extensions;
using BlazUICommunity.Utility.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BlazUICommunity.Request
{
  public  class FollowRequest : BaseRequest
    {
        /// <summary>
        /// 状态 0 正常，-1已取消
        /// </summary>
        [ExpressionQuery(OperationType.Equal)]
        public int? Status { get; set; } = null;

        /// <summary>
        /// 关注时间开始
        /// </summary>
        [ExpressionQuery(OperationType.GreaterThanOrEqual , "FollowTime")]
        public DateTime FollowTimeStart { get; set; }
        /// <summary>
        /// 关注时间结束
        /// </summary>
        [ExpressionQuery(OperationType.LessThanOrEqual , "FollowTime")]
        public DateTime FollowTimeEnd { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [ExpressionQuery(OperationType.Equal )]
        public int UserId { get; set; }


    }
}
