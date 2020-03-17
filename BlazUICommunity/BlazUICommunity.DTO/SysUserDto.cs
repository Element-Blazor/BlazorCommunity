namespace Blazui.Community.DTO
{
    using System;

    public class SysUserDto : BaseDto
    {
        public string Name { get; set; }

        public string Password { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public long? QQ { get; set; }

        public string WeChat { get; set; }

        public byte? Sex { get; set; }

        public DateTime? LastLoginTime { get; set; }
    }
}