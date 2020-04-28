using Blazui.Community.Enums;
using System.Threading.Tasks;

namespace Blazui.Community.Api.Service
{
    /// <summary>
    ///
    /// </summary>
    public interface IMessageService
    {
        /// <summary>
        /// 邮箱登录-发送验证码
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Content"></param>
        /// <returns></returns>
        Task<bool> SendVerifyCodeForLoginWithEmailAsync(string Email, string Content);
        /// <summary>
        /// 绑定邮箱-发送验证码
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Content"></param>
        /// <returns></returns>
        Task<bool> SendVerifyCodeForBindEmailAsync(string Email, string Content);
        /// <summary>
        /// 找回密码 -发送验证码
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Content"></param>
        /// <returns></returns>
        Task<bool> SendVerifyCodeForRetrievePasswordAsync(string Email, string Content);
        /// <summary>
        /// 修改密码 -发送验证码
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Content"></param>
        /// <returns></returns>
        Task<bool> SendVerifyCodeForChangePasswordAsync(string Email, string Content);
        /// <summary>
        /// 发送通知给管理员，回答社区提问
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        Task<bool> SendEmailToManagerForAnswerAsync(string Content);

        /// <summary>
        /// 发送通知给主贴发帖人，有新的回复
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Content"></param>
        /// <returns></returns>
        Task<bool> SendEmailToTopicCreatorAsync(string Email, string Content);


    }
}