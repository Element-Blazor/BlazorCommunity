
using System.ComponentModel.DataAnnotations;

namespace BlazorCommunity.Request
{
    public class UsersRequestCondition : BaseRequestCondition
    {
        /// <summary>
        /// 用户账号
        /// </summary>
        [StringLength(20)]
        [ExpressionQuery(OperationType.Like)]
        public string UserName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [ExpressionQuery(OperationType.Like)]
        public string Email { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [ExpressionQuery(OperationType.Like)]
        public string PhoneNumber { get; set; }
    }
}