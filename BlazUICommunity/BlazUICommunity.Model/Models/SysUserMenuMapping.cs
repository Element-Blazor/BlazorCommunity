namespace BlazUICommunity.Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SysUserMenuMapping")]
    public partial class SysUserMenuMapping
    {
        public int Id { get; set; }

        public int SysUserId { get; set; }

        public int SysMenuId { get; set; }
    }
}
