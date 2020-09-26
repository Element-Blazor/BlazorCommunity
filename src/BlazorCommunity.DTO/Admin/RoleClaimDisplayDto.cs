using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorCommunity.DTO.Admin
{
   public class RoleClaimDisplayDto
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string ClaimType { get; set; }
        public string ClaimName { get; set; }
    }
}
