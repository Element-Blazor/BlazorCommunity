using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Model
{
    public class BindMobileModel
    {
        public string Mobile { get; set; }

        public string VerifyCode { get; set; }
    }
}
