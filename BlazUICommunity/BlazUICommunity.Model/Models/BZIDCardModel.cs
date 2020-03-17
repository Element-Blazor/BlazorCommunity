using System.ComponentModel.DataAnnotations.Schema;

namespace Blazui.Community.Model.Models
{
    [Table("BZIDCard")]
    public partial class BZIDCardModel : BaseModel
    {
        public string IdentityNo { get; set; }
        public string UserName { get; set; }
        public string PhotoFront { get; set; }
        public string PhotoBehind { get; set; }

        [Column("UserId", TypeName = "varchar(36)")]
        public string UserId { get; set; }
    }
}