using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazUICommunity.DTO
{
    public partial class BZUserDto
    {


        /// <summary>
        /// 用户账号
        /// </summary>
        [StringLength(20)]
        public string Account { get; set; }
        /// <summary>
        ///  密码--MD5
        /// </summary>
        [StringLength(32)]
        public string Cypher { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [StringLength(50)]
        public string NickName { get; set; }
        /// <summary>
        /// 手机号码-11位
        /// </summary>
        [StringLength(11)]
        public string Mobile { get; set; }
        /// <summary>
        /// 头像
        /// </summary>

        public string Avatar { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
#nullable enable
        public string? Email { get; set; }
#nullable disable
        /// <summary>
        /// 性别0 男，1女，2呃...
        /// </summary>

        public int? Sex { get; set; }
        /// <summary>
        /// 座右铭
        /// </summary>
        public string Signature { get; set; }
        /// <summary>
        /// 注册日期
        /// </summary>
        public DateTime RegisterDate { get; set; }
        /// <summary>
        /// 状态0正常，-1已禁用
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public int? Level { get; set; }
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
#nullable enable
        public string? LastLoginAddr { get; set; }
#nullable disable

    }
}
