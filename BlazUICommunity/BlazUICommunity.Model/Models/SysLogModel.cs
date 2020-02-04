namespace Blazui.Community.Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SysLog")]
    public partial class SysLogModel : BaseModel
    {

        [Required]
        [StringLength(36)]
        public string UserName { get; set; }

        /// <summary>
        /// ธลาช
        /// </summary>
        [Required]
        [StringLength(1000)]
        public string Introduction { get; set; }
        /// <summary>
        /// ฯ๊ว้
        /// </summary>
        [StringLength(4000)]
        public string Detail { get; set; }

        public byte LogType { get; set; }

        public DateTime CreateTime { get; set; }

        public int CreatorId { get; set; }

        public DateTime? LastModifyTime { get; set; }

        public int? LastModifierId { get; set; }
    }
}
