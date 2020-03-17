using Blazui.Community.Enums;
using Blazui.Community.Utility;
using Blazui.Community.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Blazui.Community.DTO.Admin
{
    public class TopicDisplayDto
    {

        public string Id { get; set; }
        public string UserName { get; set; }

        public string Content { get; set; }

        public int Category { get; set; }

        public string CategoryDisplay => ((TopicCategory)Category).Description();

        public string Title { get; set; }
        public int Status { get; set; }
        public DateTime CreateDate { get; set; }

        [NotMapped]
        [AutoNotMap]
        public string StatusDisplay => ((DelStatus)Status).Description();
        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        [AutoNotMap]
        public string LastModifyDateDisplay => CreateDate.ConvertToDateDiffStr();

        public int Good { get; set; }
        [NotMapped]
        [AutoNotMap]
        public string GoodDisplay => Good switch
        {
            0 => "否",
            1 => "是",
            _ => "Unkown"
        };

        public int Top { get; set; }
        [NotMapped]
        [AutoNotMap]
        public string TopDisplay => Top switch
        {
            0 => "否",
            1 => "是",
            _ => "Unkown"
        };

        public int ReplyCount { get; set; }

        public string CreatorId { get; set; }

    }
}
