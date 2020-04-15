using Blazui.Community.AutoMapperExtensions;
using Blazui.Community.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazui.Community.DTO
{
    public class BZTopicDto : BaseDto
    {
        /// <summary>
        /// 标题
        /// </summary>
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
        public string VersionId { get; set; }

        [AutoNotMap]
        public string UserName { get; set; }

        [AutoNotMap]
        public string Avator { get; set; }

        [AutoNotMap]
        public string Signature { get; set; }

        [AutoNotMap]
        public string NickName { get; set; }

        [NotMapped]
        [AutoNotMap]
        public string GoodDisplay => Good switch
        {
            0 => "否",
            1 => "是",
            _ => "Unkown"
        };

        [NotMapped]
        [AutoNotMap]
        public string TopDisplay => Top switch
        {
            0 => "否",
            1 => "是",
            _ => "Unkown"
        };

        [NotMapped]
        [AutoNotMap]
        public string CategoryDisplay => ((TopicCategory)Category).Description();

        [AutoNotMap]
        [NotMapped]
        public string VerName { get; set; }
    }

    public class SeachTopicDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
    }
}