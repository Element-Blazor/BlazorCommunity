using Blazui.Community.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Community.DTO
{
   public class ReplyDto: BZReplyDto
    {
        public string Title { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
    }
}
