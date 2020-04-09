using System.Collections.Generic;

namespace Blazui.Community.Api.Configuration
{
    public class EmailNoticeOptions
    {
        public EmailNoticeModel[] EmailNotices { get; set; }
    }

    public class EmailNoticeModel
    {
        /// <summary>
        /// 通知邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 权重
        /// </summary>
        public int Weight { get; set; }
    }

    public class Test
    {
        public int A { get; set; }
    }

}
