using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorCommunity.DTO.Admin
{
    public class RoleClaimDto
    {
        public string RoleId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
