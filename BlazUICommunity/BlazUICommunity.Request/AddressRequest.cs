using BlazUICommunity.Utility.Extensions;
using BlazUICommunity.Utility.Request;
using System;

namespace BlazUICommunity.Request
{
    public class AddressRequest : BaseRequest
    {

        /// <summary>
        /// 国家
        /// </summary>
        [ExpressionQuery(OperationType.Like)]
        public string Country { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        [ExpressionQuery(OperationType.Like)]
        public string Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        [ExpressionQuery(OperationType.Like)]
        public string City { get; set; }
        /// <summary>
        /// 地区
        /// </summary>
        [ExpressionQuery(OperationType.Like)]
        public string District { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [ExpressionQuery(OperationType.Equal)]
        public int? UserId { get; set; }
    }
}
