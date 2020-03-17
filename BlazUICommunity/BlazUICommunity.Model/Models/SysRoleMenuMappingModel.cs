namespace Blazui.Community.Model.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SysRoleMenu")]
    public partial class SysRoleMenuMappingModel : BaseModel
    {
        public int SysRoleId { get; set; }

        public int SysMenuId { get; set; }
    }
}