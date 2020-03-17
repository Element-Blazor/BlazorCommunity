using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazui.Community.Model.Models
{
    [Table("BZAutho")]
    public partial class BZAutho2Model : BaseModel
    {
        /// <summary>
        /// 登录类型：0：QQ，1：微信，2：微博，...
        /// </summary>
        public int OauthType { get; set; }

        /// <summary>
        /// OauthName
        /// </summary>
        public string OauthName { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        [StringLength(100)]
        public string NickName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Column("UserId", TypeName = "varchar(36)")]
        public string UserId { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string HomePage { get; set; }
    }
}