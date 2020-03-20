using Blazui.Community.AutoMapperExtensions;
using Blazui.Community.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazui.Community.DTO.Admin
{
    public class ReplyDisplayDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public int Status { get; set; }
        public string Author { get; set; }

        public string CreatorId { get; set; }
        public string TopicId { get; set; }

        [NotMapped]
        [AutoNotMap]
        public string StatusDisplay => ((DelStatus)Status).Description();

        public string Title { get; set; }
        public string NickName { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
    }
}