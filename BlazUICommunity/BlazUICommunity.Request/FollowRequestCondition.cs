

namespace Blazui.Community.Request
{
    public  class FollowRequestCondition : BaseRequestCondition
    {
        
        /// <summary>
        /// 用户ID
        /// </summary>
        [ExpressionQuery(OperationType.Equal )]
        public string CreatorId { get; set; }

        public string TopicTitle { get; set; }

    }
}
