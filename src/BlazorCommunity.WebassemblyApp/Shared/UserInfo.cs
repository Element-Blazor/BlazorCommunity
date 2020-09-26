using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Community.WebassemblyApp.Shared
{
    public class UserInfo
    {
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }
        public Dictionary<string, string> ExposedClaims { get; set; }
    }
}
