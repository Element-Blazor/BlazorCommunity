﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazui.Community.DTO
{
    /// <summary>
    /// 我的关注 
    /// </summary>
    public partial class BZFollowDto
    {
        public int Id { get; set; }
        /// <summary>
        /// 主题帖ID
        /// </summary>
        public int TopicId { get; set; }
        /// <summary>
        /// 状态 0 正常，-1已取消
        /// </summary>
        public int? Status { get; set; }
       /// <summary>
       /// 关注时间
       /// </summary>
        public DateTime FollowTime { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

    }
}
