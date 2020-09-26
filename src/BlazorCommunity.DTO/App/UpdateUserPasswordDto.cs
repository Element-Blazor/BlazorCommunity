using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorCommunity.DTO.App
{
   public class UpdateUserPasswordDto
    {
        public string UserId { get; set; }
        public string NewPassword { get; set; }
    }
}
