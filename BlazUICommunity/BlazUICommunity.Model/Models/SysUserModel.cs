namespace Blazui.Community.Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SysUser")]
    public partial class SysUserModel : BaseModel
    {

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        [Required]
        [StringLength(64)]
        public string Password { get; set; }


        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(20)]
        public string Mobile { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        public long? QQ { get; set; }

        [StringLength(50)]
        public string WeChat { get; set; }

        public byte? Sex { get; set; }

        public DateTime? LastLoginTime { get; set; }

    }
}
