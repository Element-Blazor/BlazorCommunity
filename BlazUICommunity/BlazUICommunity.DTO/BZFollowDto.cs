using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazui.Community.DTO
{
    /// <summary>
    /// 我的关注 
    /// </summary>
    public  class BZFollowDto : BaseDto
    {
        /// <summary>
        /// 主题帖ID
        /// </summary>
        public string TopicId { get; set; }
    }
}
