using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazui.Community.Model.Models
{
    [Table("BZUser")]
    public partial class BZUserModel : IdentityUser<string>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override string Id { get => base.Id; set => base.Id = value; }
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
        [Column("LastLoginDate", TypeName = "timestamp")]
        public DateTime LastLoginDate { get; set; }
        /// <summary>
        /// 最后登录类型-0 默认账号登录，其他由第三方账号类型决定
        /// </summary>
        public int LastLoginType { get; set; }
        /// <summary>
        /// 最后登录位置,具体位置，或Ip地址
        /// </summary>
        public string LastLoginAddr { get; set; } = "";



        /// <summary>
        /// 状态0正常，-1已禁用
        /// </summary>
        public int Status { get; set; } = 0;
        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("CreateDate", TypeName = "timestamp")]
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        [Column("LastModifyDate", TypeName = "timestamp")]
        public DateTime? LastModifyDate { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [Column("CreatorId", TypeName = "varchar(36)")]
        public string CreatorId { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        [Column("LastModifierId", TypeName = "varchar(36)")]
        public string? LastModifierId { get; set; }
    }
}
