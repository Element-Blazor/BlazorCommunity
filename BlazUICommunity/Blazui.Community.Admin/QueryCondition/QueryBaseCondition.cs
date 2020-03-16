﻿using Blazui.Community.Enums;
using Blazui.Community.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Admin.QueryCondition
{
    public class QueryBaseCondition
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 15;


        /// <summary>
        /// 发布时间开始
        /// </summary>
        public DateTime? CreateDateStart { get; set; }
        /// <summary>
        /// 发布时间结束
        /// </summary>
        public DateTime? CreateDateEnd { get; set; }

        /// <summary>
        /// 状态 0正常，-1 删除
        /// </summary>
        public DelStatus? Status { get; set; }
    }
}
