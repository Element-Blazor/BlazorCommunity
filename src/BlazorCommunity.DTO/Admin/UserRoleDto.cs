using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorCommunity.DTO.Admin
{
   public class UserRoleDto
    {
        public string UserId { get; set; }
        public List<string> RoleIds { get; set; }
    }
}
