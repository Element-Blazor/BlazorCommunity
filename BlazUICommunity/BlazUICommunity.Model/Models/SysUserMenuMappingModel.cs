namespace Blazui.Community.Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SysUserMenu")]
    public partial class SysUserMenuMappingModel : BaseModel
    {

        public int SysUserId { get; set; }

        public int SysMenuId { get; set; }
    }
}
