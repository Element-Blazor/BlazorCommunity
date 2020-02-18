using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazui.Community.DTO
{

    /// <summary>
    /// 回帖
    /// </summary>
    public partial class BZReplyDto 
    {
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
    }

   public class BZReplyDtoWithUser: BZReplyDto
    {
        public string Title { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avator { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public string LastModifyTime { get; set; }

        [NotMapped]
        public string StatusDisplay
        {
            get
            {
                return Status switch
                {
                    0 => "正常",
                    -1 => "已删除",
                    _ => "正常"
                };
            }
        }
    }
}
