using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Blazui.Community.Model.Models
{
    [Table("Verify")]
    public   class BzVerifyCodeModel:BaseModel
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
        public int UserId { get; set; }
        /// <summary>
        /// 验证码类型 0：登录，1：修改密码，2：绑定手机
        /// </summary>
        public  int VerifyType { get; set; }

        /// <summary>
        /// 是否已过期
        /// </summary>
        [NotMapped]
        public bool IsExpired { get { return ExpireTime<DateTime.Now; } set { }}
    }
}
