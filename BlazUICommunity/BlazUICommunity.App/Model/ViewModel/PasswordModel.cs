namespace Blazui.Community.App.Model
{
    public class PasswordModel
    {
        /// <summary>
        /// 验证码
        /// </summary>
        public string VerifyCode { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        public string OldPassword { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// 0：绑定手机，1：修改密码
        /// </summary>
        public int VerifyType { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Email { get; set; }
    }
}