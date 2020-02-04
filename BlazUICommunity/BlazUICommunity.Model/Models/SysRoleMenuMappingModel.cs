namespace Blazui.Community.Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SysRoleMenuMapping")]
    public partial class SysRoleMenuMappingModel : BaseModel
    {

        public int SysRoleId { get; set; }

        public int SysMenuId { get; set; }
    }
}
