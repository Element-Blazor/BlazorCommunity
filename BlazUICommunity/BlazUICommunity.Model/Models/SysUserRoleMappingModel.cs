namespace Blazui.Community.Model.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SysUserRole")]
    public partial class SysUserRoleMappingModel : BaseModel
    {
        public int SysUserId { get; set; }

        public int SysRoleId { get; set; }
    }
}