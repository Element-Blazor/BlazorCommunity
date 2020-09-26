using BlazorCommunity.Enums;

namespace BlazorCommunity.WasmApp.Model.Condition
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