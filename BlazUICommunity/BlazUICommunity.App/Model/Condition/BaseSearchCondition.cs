using Blazui.Community.Request;

namespace Blazui.Community.App.Model.Condition
{
    public class BaseSearchCondition
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string CreatorId { get; set; }
        public PageInfo PageInfo { get; set; } = new PageInfo() { PageSize = 5, PageIndex = 1 };
    }
}
