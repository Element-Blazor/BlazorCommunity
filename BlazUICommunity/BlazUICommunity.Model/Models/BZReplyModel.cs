using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazUICommunity.Model.Models
{

    /// <summary>
    /// 回帖
    /// </summary>
    [Table("Reply")]
    public partial class BZReplyModel
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 回帖时间
        /// </summary>
        public DateTime PublishTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifyTime { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 回复的主题帖ID
        /// </summary>
        public int TopicId { get; set; }
        /// <summary>
        /// 状态 0正常-1删除
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 点赞数量
        /// </summary>
        public int? Favor { get; set; }

        public virtual BZTopicModel Topic { get; set; }
    }
}
