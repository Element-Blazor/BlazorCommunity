﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazui.Community.Model.Models
{
    /// <summary>
    /// 我的关注 
    /// </summary>
    [Table("BZFollow")]
    public partial class BZFollowModel : BaseModel
    {
        /// <summary>
        /// 主题帖ID
        /// </summary>
        [Column("TopicId", TypeName = "varchar(36)")]
        public string TopicId { get; set; }

     
    }
}
