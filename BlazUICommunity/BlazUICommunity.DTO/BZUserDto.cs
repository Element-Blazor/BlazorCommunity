using Blazui.Community.Enums;
using Blazui.Community.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazui.Community.DTO
{
    public  class BZUserDto :BaseDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [StringLength(50)]
        public string NickName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>

        public string Avator { get; set; } = "";

        /// <summary>
        /// 性别0 男，1女，2呃...
        /// </summary>

        public int Sex { get; set; } = 0;
        /// <summary>
        /// 座右铭
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public int Level { get; set; } = 0;
        /// <summary>
        /// 积分
        /// </summary>
        public int? Points { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime LastLoginDate { get; set; }
        /// <summary>
        /// 最后登录类型-0 默认账号登录，其他由第三方账号类型决定
        /// </summary>
        public int LastLoginType { get; set; }
        /// <summary>
        /// 最后登录位置,具体位置，或Ip地址
        /// </summary>
        public string LastLoginAddr { get; set; } = "";


        [NotMapped]
        [AutoNotMap]
        public string SexDisplay => ((Sex)this.Sex).Description();

    }
}
