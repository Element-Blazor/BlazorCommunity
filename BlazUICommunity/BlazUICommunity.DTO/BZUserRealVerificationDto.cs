using System;
using System.Collections.Generic;

namespace BlazUICommunity.Model.Models
{

    public partial class BZUserRealVerificationDto 
    {
        public string IdentityNo { get; set; }
        public string UserName { get; set; }
        public string PhotoFront { get; set; }
        public string PhotoBehind { get; set; }
        public int UserId { get; set; }
    }
}
