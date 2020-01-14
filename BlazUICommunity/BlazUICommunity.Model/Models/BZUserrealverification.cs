using System;
using System.Collections.Generic;

namespace BlazUICommunity.Model.Models
{
    [Obsolete("暂时不用")]
    public partial class BZUserRealverification
    {
        public int Id { get; set; }
        public string IdentityNo { get; set; }
        public string UserName { get; set; }
        public string PhotoFront { get; set; }
        public string PhotoBehind { get; set; }
        public int UserId { get; set; }

        public virtual BZUserModel User { get; set; }
    }
}
