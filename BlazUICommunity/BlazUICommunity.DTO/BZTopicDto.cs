using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazui.Community.DTO
{
    public partial class BZTopicDto 
    {
      
     
       /// <summary>
       /// 标题
       /// </summary>
        [StringLength(100)]
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime PublishTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifyTime { get; set; }
        /// <summary>
        /// 发帖人ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 状态 0正常，-1 删除，1已结帖
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 人气--浏览量
        /// </summary>
        public int? Hot { get; set; }
        /// <summary>
        /// 是否置顶0否-1置顶
        /// </summary>
        public int? Top { get; set; }
        /// <summary>
        /// 是否精华帖0否，1-是
        /// </summary>
        public int? Good { get; set; }
        /// <summary>
        /// 主题帖类型 0：提问，1：分享，2：讨论，3：建议，4：公告
        /// </summary>
        public int? TopicType { get; set; }
        /// <summary>
        /// 回帖数量
        /// </summary>
        public int? ReplyCount { get; set; }
    }
}
