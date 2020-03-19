using Blazui.Community.Enums;

namespace Blazui.Community.App.Model
{
    public class NewTopicModel
    {
        /// <summary>
        /// 帖子标题
        /// </summary>

        public string Title { get; set; }
        /// <summary>
        /// 类别
        /// </summary>

        public string CategoryDisplay => Category.Description();

        public TopicCategory Category { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        public string ReleaseTime { get; set; }

        public string VerNo { get; set; }
        public string Id { get; internal set; }

        public ProjectType projectType { get; set; }
    }
}