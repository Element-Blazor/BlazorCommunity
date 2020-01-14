namespace BlazUICommunity.Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SysMenu")]
    public partial class SysMenu
    {
        public int Id { get; set; }

        public int ParentId { get; set; }

        [Required]
        [StringLength(100)]
        public string Text { get; set; }

        [StringLength(500)]
        public string Url { get; set; }

        public byte MenuLevel { get; set; }

        public byte MenuType { get; set; }

        [StringLength(20)]
        public string MenuIcon { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(1000)]
        public string SourcePath { get; set; }

        public int Sort { get; set; }

        public byte Status { get; set; }

        public DateTime CreateTime { get; set; }

        public int CreatorId { get; set; }

        public DateTime? LastModifyTime { get; set; }

        public int? LastModifierId { get; set; }
    }
}
