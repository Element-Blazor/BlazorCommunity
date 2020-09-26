namespace BlazorCommunity.Request
{
    public class BannerRequestCondition : BaseRequestCondition
    {
        [ExpressionQuery(OperationType.Like)]
        public string Title { get; set; } = null;
    }
}