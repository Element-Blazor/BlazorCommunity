namespace Blazui.Community.Model.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SysLog")]
    public partial class SysLogModel : BaseModel
    {
        [Required]
        [StringLength(36)]
        public string UserName { get; set; }

        /// <summary>
        /// ��Ҫ
        /// </summary>
        [Required]
        [StringLength(1000)]
        public string Introduction { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        [StringLength(4000)]
        public string Detail { get; set; }

        public byte LogType { get; set; }
    }
}