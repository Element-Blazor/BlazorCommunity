using Blazui.Community.Utility.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Admin.QueryCondition
{
    public class QueryUserCondition : BaseRequest
    {
        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 手机号码-11位
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Email { get; set; }

        public DateTime? RegisterDateStart { get; set; }
        public DateTime? RegisterDateEnd { get; set; }
    }
}
