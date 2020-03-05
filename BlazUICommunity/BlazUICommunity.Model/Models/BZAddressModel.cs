using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazui.Community.Model.Models
{

    [Table("BZAddress")]
    public partial class BZAddressModel: BaseModel
    {
    
        /// <summary>
        /// 国家
        /// </summary>
        [StringLength(20)]
        public string Country { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        [StringLength(20)]
        public string Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        [StringLength(20)]
        public string City { get; set; }
        /// <summary>
        /// 地区
        /// </summary>
        [StringLength(20)]
        public string District { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>

        [Column("UserId", TypeName = "varchar(36)")]
        public string UserId { get; set; }

    }
}
