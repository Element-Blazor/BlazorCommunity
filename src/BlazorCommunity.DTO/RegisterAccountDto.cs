﻿using System.ComponentModel.DataAnnotations;

namespace BlazorCommunity.DTO
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

        public string QQ { get; set; }

        public string Email { get; set; }
    }
}