

namespace Blazui.Community.Request
{
    public  class TopicRequestCondition : BaseRequestCondition
    {
        /// <summary>
        /// 标题
        /// </summary>
        [ExpressionQuery(OperationType.Like)]
        public string Title { get; set; }


        /// <summary>
        /// 发帖人ID
        /// </summary>
        [ExpressionQuery(OperationType.Equal)]
        public string CreatorId { get; set; } = null;

        /// <summary>
        /// 是否置顶0否-1置顶
        /// </summary>
        [ExpressionQuery(OperationType.Equal)]
        public int? Top { get; set; } = null;
        /// <summary>
        /// 是否精华帖0否，1-是
        /// </summary>  
        [ExpressionQuery(OperationType.Equal)]
        public int? Good { get; set; } = null;
        /// <summary>
        /// 主题帖类型 0：提问，1：分享，2：讨论，3：建议，4：公告
        /// </summary>
        [ExpressionQuery(OperationType.Equal)]
        public int? Category { get; set; } = null;


    }
}
