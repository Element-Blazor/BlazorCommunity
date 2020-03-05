using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazui.Community.Model.Models
{
    [Table("BZTopic")]
    public partial class BZTopicModel : BaseModel
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
        /// 人气--浏览量
        /// </summary>
        public int Hot { get; set; }
        /// <summary>
        /// 是否置顶0否-1置顶
        /// </summary>
        public int Top { get; set; }
        /// <summary>
        /// 是否精华帖0否，1-是
        /// </summary>
        public int Good { get; set; }
        /// <summary>
        /// 主题帖类型 0：提问，1：分享，2：讨论，3：建议，4：公告
        /// </summary>
        public int Category { get; set; }
        /// <summary>
        /// 回帖数量
        /// </summary>
        public int ReplyCount { get; set; }
        /// <summary>
        /// 版本Id
        /// </summary>

        [Column("VersionId", TypeName = "varchar(36)")]
        public string VersionId { get; set; } = "";



    }
}
