using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Community.Shared
{
    public class UserInfo
    {
        public string Id { get; set; }
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }
        public string Avator { get; set; }
        public string Email { get; set; }
        public Dictionary<string, string> ExposedClaims { get; set; }
    }
}
