using System;

namespace BlazorCommunity.DTO
{
    public class PersonalReplyDisplayDto
    {
        public string Id { get; set; }

        public string TopicId { get; set; }
        public string Title { get; set; }

        public string Author { get; set; }
        public DateTime CreateDate { get; set; }
        public int Status { get; set; }

        public string Content { get; set; }
    }
}