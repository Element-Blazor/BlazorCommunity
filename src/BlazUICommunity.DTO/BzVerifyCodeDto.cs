using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazui.Community.DTO
{
    public class BzVerifyCodeDto : BaseDto
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
        public string UserId { get; set; }

        /// <summary>
        /// 验证码类型 0：登录，1：修改密码，2：绑定手机
        /// </summary>
        public int VerifyType { get; set; }

        /// <summary>
        /// 是否已过期
        /// </summary>
        [NotMapped]
        public bool IsExpired { get { return ExpireTime < DateTime.Now; } set { } }
    }
}