using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazUICommunity.Model.Models
{
    /// <summary>
    /// 我的关注 
    /// </summary>
    [Table("Follow")]
    public partial class BZFollowModel
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        [Key]
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

        public virtual BZTopicModel Topic { get; set; }
        public virtual BZUserModel User { get; set; }
    }
}
