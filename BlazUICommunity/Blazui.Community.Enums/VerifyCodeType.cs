namespace Blazui.Community.Enums
{
    public enum VerifyCodeType
    {
        /// <summary>
        /// 邮箱验证码登录
        /// </summary>
        EmailLogin = 1,

        /// <summary>
        /// 手机验证码登录
        /// </summary>
        MobileLogin,

        /// <summary>
        /// 绑定手机
        /// </summary>
        MobileBind,

        /// <summary>
        /// 绑定邮箱
        /// </summary>
        EmailBind,

        /// <summary>
        /// 通过邮箱找回密码
        /// </summary>
        EmailRetrievePassword,

        /// <summary>
        /// 通过手机找回密码
        /// </summary>
        MobileRetrievePassword,

        /// <summary>
        /// 通过邮箱修改密码
        /// </summary>
        EmailChangePassword,

        /// <summary>
        /// 通过手机修改密码
        /// </summary>
        MobileChangePassword
    }
}