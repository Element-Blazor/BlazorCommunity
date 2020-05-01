using Blazui.Community.Enums;

namespace Blazui.Community.WasmApp.Model.Condition
{
    public class SearchPersonalTopicCondition : BaseSearchCondition
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