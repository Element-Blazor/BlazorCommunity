using Blazui.Community.AutoMapperExtensions;
using Blazui.Community.DateTimeExtensions;
using Blazui.Community.Enums;
using System;

namespace Blazui.Community.DTO
{
    public class PersonalFollowDisplayDto
    {
        public string Id { get; set; }

        public string CreatorId { get; set; }

        /// <summary>
        /// 帖子标题
        /// </summary>
        public string Title { get; set; }

        public string Avator { get; set; }

        /// <summary>
        /// 发表时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        ///  状态 0正常，-1 删除，1已结帖
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        ///  状态 0正常，-1 删除，1已结帖
        /// </summary>
        [AutoNotMap]
        public string StatusDisplay => ((DelStatus)Status).Description();

        /// <summary>
        /// 主题帖类型 0：提问，1：分享，2：讨论，3：建议，4：公告
        /// </summary>
        public string CategoryDisplay => Category.Description();

        public TopicCategory Category { get; set; }

        /// <summary>
        /// 是否精华
        /// </summary>
        public int Good { get; set; }

        [AutoNotMap]

        /// <summary>
        /// 是否精华
        /// </summary>
        public string GoodDisplay => Good switch
        {
            0 => "否",
            1 => "是",
            _ => "否"
        };

        /// <summary>
        /// 回帖数量
        /// </summary>
        public int ReplyCount { get; set; }

        [AutoNotMap]
        public string CreateTimeDisplay => CreateDate.ConvertToDateDiffStr();

        public string UserName { get; set; }

        public string NickName { get; set; }

        public string FollowId { get; set; }
    }
}