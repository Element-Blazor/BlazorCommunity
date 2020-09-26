using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace BlazorCommunity.DTO.Admin
{
   public class RoleClaimFormDto
    {
        public string RoleId { get; set; }

        public List<Claim> Claims { get; set; }
    }
}
