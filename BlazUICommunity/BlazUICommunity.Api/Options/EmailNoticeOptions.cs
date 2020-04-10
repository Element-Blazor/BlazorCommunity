using System.Collections.Generic;

namespace Blazui.Community.Api.Options
{
    public class EmailNoticeOptions
    {
        /// <summary>
        /// 字段名称必须与配置项一致
        /// </summary>
        public List<EmailNoticeModel> EmailNotices { get; set; }
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
