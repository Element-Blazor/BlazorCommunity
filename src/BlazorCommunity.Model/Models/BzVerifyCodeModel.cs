using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorCommunity.Model.Models
{
    [Table("BZVerify")]
    public class BzVerifyCodeModel : BaseModel
    {
        /// <summary>
        /// 验证码
        /// </summary>
        public string VerifyCode { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpireTime { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Column("UserId", TypeName = "varchar(36)")]
        public string UserId { get; set; }

        /// <summary>
        ///  验证码类型 1：邮箱验证码登录，2：手机验证码登录，3：绑定手机，4：绑定邮箱，5：通过邮箱找回密码，6：通过手机找回密码，7：通过邮箱修改密码，8：通过手机修改密码
        /// </summary>
        public int VerifyType { get; set; }

        /// <summary>
        /// 是否已过期
        /// </summary>
        [NotMapped]
        public bool IsExpired { get { return ExpireTime < DateTime.Now; } set { } }
    }
}