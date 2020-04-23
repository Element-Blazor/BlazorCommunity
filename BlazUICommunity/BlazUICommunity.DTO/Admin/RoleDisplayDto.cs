using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Community.DTO.Admin
{
   public class RoleDisplayDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class NewRoleDto
    {
        public string Name { get; set; }
    }


    public class RolesDto
    {
        public List<RoleDisplayDto> Roles { get; set; }
    }
}
