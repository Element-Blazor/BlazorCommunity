using System.ComponentModel.DataAnnotations.Schema;

namespace Blazui.Community.Model.Models
{
    /// <summary>
    /// 回帖
    /// </summary>
    [Table("BZReply")]
    public partial class BZReplyModel : BaseModel
    {
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 回复的主题帖ID
        /// </summary>
        [Column("TopicId", TypeName = "varchar(36)")]
        public string TopicId { get; set; }

        /// <summary>
        /// 点赞数量
        /// </summary>
        public int? Favor { get; set; }

        /// <summary>
        /// 是否置顶0否-1置顶
        /// </summary>
        public int Top { get; set; }

        /// <summary>
        /// 是否精华帖0否，1-是
        /// </summary>
        public int Good { get; set; }
    }
}