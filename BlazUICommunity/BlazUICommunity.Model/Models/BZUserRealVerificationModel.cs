using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazui.Community.Model.Models
{
    [Obsolete("暂时不用")]
    [Table("UserRealverification")]
    public partial class BZUserRealVerificationModel : BaseModel
    {
        public string IdentityNo { get; set; }
        public string UserName { get; set; }
        public string PhotoFront { get; set; }
        public string PhotoBehind { get; set; }
        public int UserId { get; set; }

        public virtual BZUserModel User { get; set; }
    }
}
