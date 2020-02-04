﻿using Blazui.Community.Utility.Extensions;
using Blazui.Community.Utility.Request;
using System;

namespace Blazui.Community.Request
{
    public class PointRequest : BaseRequest
    {

        /// <summary>
        /// 积分来源，1：发帖，2：回帖，3:精华帖，4：其他--规则
        /// </summary>
        [ExpressionQuery(OperationType.Equal)]
        public int? Access { get; set; } = null;

        /// <summary>
        /// 用户ID
        /// </summary>
        [ExpressionQuery(OperationType.Equal)]
        public int? UserId { get; set; }
    }
}
