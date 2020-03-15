using Blazui.Community.Enums;
using Blazui.Community.Utility;
using Blazui.Community.Utility.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazui.Community.DTO
{
    public class BaseDto
    {
        
        public string Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime LastModifyDate { get; set; }
        /// <summary>
        /// 创建人Id
        /// </summary>
        public string CreatorId { get; set; }
        /// <summary>
        /// 删除状态 0：正常，-1：已删除
        /// </summary>
       
        public int Status { get; set; } = 0;

        /// <summary>
        /// 最后修改人Id
        /// </summary>
        public string LastModifierId { get; set; } = null;
        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        [AutoNotMap]
        public string StatusDisplay => ((DelStatus)Status).Description();
        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        [AutoNotMap]
        public string LastModifyDateDisplay => CreateDate.ConvertToDateDiffStr();
   
    }
}
