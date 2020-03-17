namespace Blazui.Community.App.Model.Condition
{
    public class BaseSearchCondition
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string CreatorId { get; set; }

        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 15;
    }
}