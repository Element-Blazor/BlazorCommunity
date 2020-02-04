namespace Blazui.Community.Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class SysLogDto 
    {

        [StringLength(36)]
        public string UserName { get; set; }

        /// <summary>
        /// ��Ҫ
        /// </summary>
        [StringLength(1000)]
        public string Introduction { get; set; }
        /// <summary>
        /// ����
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
