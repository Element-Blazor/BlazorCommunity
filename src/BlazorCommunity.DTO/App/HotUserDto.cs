using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BlazorCommunity.DTO
{
    public class HotUserDto
    {
        //public string Id { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public DateTime LastLoginDate { get; set; }
        public long TopicCount { get; set; }
        public long ReplyCount { get; set; }

        [NotMapped]
        public bool Show { get; set; } = false;
    }
}
