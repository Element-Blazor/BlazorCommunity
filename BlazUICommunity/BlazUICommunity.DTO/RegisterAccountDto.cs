using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Blazui.Community.DTO
{

    public class RegisterAccountDto
    {
        [Required]
        public string UserAccount { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public string VerifyCode { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
