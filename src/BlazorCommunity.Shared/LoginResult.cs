using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorCommunity.Shared
{
   public class LoginResult
    {
        public  bool Successful { get; set; }
        public TokenResult  Token { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
