using System.ComponentModel;

namespace BlazorCommunity.Enums
{
    public enum UploadPath
    {
        /// <summary>
        /// 上传头像地址
        /// </summary>
        [Description("Avator")]
        Avator,

        /// <summary>
        /// 帖子正文上传图片
        /// </summary>
        [Description("Topic")]
        Topic,

        /// <summary>
        /// 后台上传Banner
        /// </summary>
        [Description("Banner")]
        Banner,

        /// <summary>
        /// 其他
        /// </summary>
        [Description("Other")]
        Other
    }
}