using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Model
{
    public class BindEmailModel
    {
        public string Email { get; set; }

        public string VerifyCode { get; set; }
    }
}
