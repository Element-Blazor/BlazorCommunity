using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Community.DTO.Admin
{
   public class UserRoleDto
    {
        public string UserId { get; set; }
        public List<string> RoleIds { get; set; }
    }
}
