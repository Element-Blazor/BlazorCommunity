using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorCommunity.Model
{
    public class BaseModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [StringLength(36)]
        [Required]
        public string Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("CreateDate", TypeName = "timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Column("LastModifyDate", TypeName = "timestamp")]
        public DateTime? LastModifyDate { get; set; }

        /// <summary>
        /// 创建人Id
        /// </summary>
        [Column("CreatorId", TypeName = "varchar(36)")]
        public string CreatorId { get; set; }

        /// <summary>
        /// 删除状态 0：正常，-1：已删除
        /// </summary>
        [Column("Status", TypeName = "tinyint")]
        public int Status { get; set; } = 0;

        /// <summary>
        /// 最后修改人Id
        /// </summary>
        [Column("LastModifierId", TypeName = "varchar(36)")]
        public string LastModifierId { get; set; } = null;
    }
}