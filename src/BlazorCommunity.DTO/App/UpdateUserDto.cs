using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorCommunity.DTO.App
{
   public class UpdateUserDto
    {
        public string Id { get; set; }
        public string NickName { get; set; }
        public string Signature { get; set; }
        public int Sex { get; set; }
    }
}
