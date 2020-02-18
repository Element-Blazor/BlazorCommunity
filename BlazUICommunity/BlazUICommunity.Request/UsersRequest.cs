using Blazui.Community.Utility.Extensions;
using Blazui.Community.Utility.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Blazui.Community.Request
{
  public  class UsersRequest:BaseRequest
    {
        /// <summary>
        /// 用户账号
        /// </summary>
        [StringLength(20)]
        [ExpressionQuery(OperationType.Like)]
        public string UserName { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        [ExpressionQuery(OperationType.Like)]
        public string Email { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [ExpressionQuery(OperationType.Like)]
        public string PhoneNumber { get; set; }
  
        /// <summary>
        /// 注册日期
        /// </summary>
        [ExpressionQuery(OperationType.GreaterThanOrEqual, "RegisterDate")]
        public DateTime? RegisterDateStart { get; set; }
        /// <summary>
        /// 注册日期
        /// </summary>
        [ExpressionQuery(OperationType.LessThanOrEqual , "RegisterDate")]
        public DateTime? RegisterDateEnd { get; set; }
        /// <summary>
        /// 状态0正常，-1已禁用
        /// </summary>
        [ExpressionQuery(OperationType.Equal)]
        public int? Status { get; set; } 
       
   
    }
}
