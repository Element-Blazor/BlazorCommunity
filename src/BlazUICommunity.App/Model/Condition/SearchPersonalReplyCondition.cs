using Blazui.Community.Enums;

namespace Blazui.Community.App.Model.Condition
{
    public class SearchPersonalReplyCondition : BaseSearchCondition
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public TopicCategory? Category { get; set; }
    }
}