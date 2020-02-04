using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Model
{
    public class ArticleModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public TopicType TopicType { get; set; }
    }

    /// <summary>
    /// 主题帖类型
    /// </summary>
    public enum TopicType
    {
        /// <summary>
        /// 0：提问
        /// </summary>
        [Description("提问")]
        Ask,
        /// <summary>
        /// 1：分享
        /// </summary>
        [Description("分享")]
        Share,
        /// <summary>
        /// 2：讨论
        /// </summary>
        [Description("讨论")]
        Discuss,
        /// <summary>
        /// 3：建议
        /// </summary>
        [Description("建议")]
        Suggest,
        /// <summary>
        /// 4：公告
        /// </summary>
        [Description("公告")]
        Notice

    }
}
