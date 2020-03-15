using Blazui.Community.Enums;
using Blazui.Community.Utility.Extensions;
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

        /// <summary>
        /// 是否置顶0否-1置顶
        /// </summary>
        public int Top { get; set; }
        /// <summary>
        /// 是否精华帖0否，1-是
        /// </summary>
        public int Good { get; set; }

        [AutoNotMap]
        public string UserName { get; set; }
        [AutoNotMap]
        public string NickName { get; set; }
        [AutoNotMap]
        public string UserId { get; set; }
        [AutoNotMap]
        public string Avator { get; set; }

        [NotMapped]
        [AutoNotMap]
        public string GoodDisplay => ((SwitchStatus)Good).Description();
        [NotMapped]
        [AutoNotMap]
        public string TopDisplay => ((SwitchStatus)Top).Description();

        /// <summary>
        /// 是否当前用户的回帖
        /// </summary>
        [NotMapped]
        public bool IsMySelf { get; set; }
        /// <summary>
        /// 是否可以修改
        /// </summary>
        [NotMapped]
        public bool ShoudEdit { get; set; }
        /// <summary>
        /// 修改之前的内容
        /// </summary>
        [NotMapped]
        public string OriginalContent { get; set; }
    }

}
