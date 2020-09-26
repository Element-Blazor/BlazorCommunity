using System.ComponentModel;

namespace BlazorCommunity.Enums
{
    public class MyClass
    {
    }

    /// <summary>
    /// 主题帖类型
    /// </summary>
    public enum TopicCategory
    {
        /// <summary>
        /// -1：首页
        /// </summary>
        //[Description("首页")]
        //Home=-1,
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
        Notice,
        /// <summary>
        /// 5：教程
        /// </summary>
        [Description("教程")]
        Doc
    }
}