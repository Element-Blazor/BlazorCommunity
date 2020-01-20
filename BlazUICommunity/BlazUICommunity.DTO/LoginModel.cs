using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BlazUICommunity.DTO
{
    public class LoginModel
    {
        [Required]
        public string Account { get; set; }

        [Required]
        public string Pw { get; set; }
    }
}
