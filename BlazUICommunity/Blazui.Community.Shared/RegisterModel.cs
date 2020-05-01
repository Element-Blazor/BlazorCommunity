using System.ComponentModel.DataAnnotations;

namespace Blazui.Community.Shared
{
    public class RegisterModel
    {
        [Required]
        public string Account { get; set; }
        [Required]
        public string Password { get; set; }

        public string Email { get; set; }

        public string NickName { get; set; }

        public int Sex { get; set; }

        public string Mobile { get; set; }
    }
}
