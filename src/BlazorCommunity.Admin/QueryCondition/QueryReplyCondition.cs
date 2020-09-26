namespace BlazorCommunity.Admin.QueryCondition
{
    public class QueryReplyCondition : BaseQueryCondition
    {
        /// <summary>
        /// 发帖人ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 回复的主题帖ID
        /// </summary>
        public object TopicId { get; set; }

        public string UserName { get; set; }

        public string Title { get; set; }
    }
}