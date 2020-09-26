using BlazorCommunity.AutoMapperExtensions;
using BlazorCommunity.DateTimeExtensions;
using BlazorCommunity.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorCommunity.DTO.Admin
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

        public string RoleId { get; set; }
        [NotMapped]
        [AutoNotMap]
        public string RoleName { get; set; }
    }
}