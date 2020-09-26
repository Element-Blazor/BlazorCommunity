using System.ComponentModel.DataAnnotations;

namespace BlazorCommunity.Shared
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
