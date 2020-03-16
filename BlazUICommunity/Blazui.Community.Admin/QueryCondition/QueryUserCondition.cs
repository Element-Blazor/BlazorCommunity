﻿using Blazui.Community.Request;
using System;

namespace Blazui.Community.Admin.QueryCondition
{
    public class QueryUserCondition : QueryBaseCondition
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


    }
}
