using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazui.Community.DTO
{

    /// <summary>
    /// 回帖
    /// </summary>
    public class BZReplyDto : BaseDto
    {
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 回复的主题帖ID
        /// </summary>
        public string TopicId { get; set; }
        /// <summary>
        /// 点赞数量
        /// </summary>
        public int? Favor { get; set; }
        public string Avator { get; set; }

        /// <summary>
        /// 是否置顶0否-1置顶
        /// </summary>
        public int Top { get; set; }
        /// <summary>
        /// 是否精华帖0否，1-是
        /// </summary>
        public int Good { get; set; }

        public string UserName { get; set; }
        public string NickName { get; set; }
        public string UserId { get; set; }
        [NotMapped]
        public string GoodDisplay
        {
            get
            {
                return Good switch
                {
                    0 => "否",
                    1 => "是",
                    _ => "Unkown"
                };
            }
        }
        [NotMapped]
        public string TopDisplay
        {
            get
            {
                return Top switch
                {
                    0 => "否",
                    1 => "是",
                    _ => "Unkown"
                };
            }
        }
        [NotMapped]
        public string LastModifyTimeDisplay { get; set; }

        [NotMapped]
        public bool IsMySelf { get; set; }

        [NotMapped]
        public bool ShoudEdit { get; set; }
        [NotMapped]
        public string OriginalContent { get; set; }
    }


    public class MyReplyDto
    {
        public List<BZReplyDto> Replys { get; set; }
        public int Total { get; set; }
    }
}
