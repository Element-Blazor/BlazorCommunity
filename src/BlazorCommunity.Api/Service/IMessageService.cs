using BlazorCommunity.Enums;
using System.Threading.Tasks;

namespace BlazorCommunity.Api.Service
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
        /// <param name="Code"></param>
        /// <returns></returns>
        Task<bool> SendVerifyCodeForLoginWithEmailAsync(string Email, string Code);
        /// <summary>
        /// 绑定邮箱-发送验证码
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        Task<bool> SendVerifyCodeForBindEmailAsync(string Email, string Code);
        /// <summary>
        /// 找回密码 -发送验证码
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        Task<bool> SendVerifyCodeForRetrievePasswordAsync(string Email, string Code);
        /// <summary>
        /// 修改密码 -发送验证码
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        Task<bool> SendVerifyCodeForChangePasswordAsync(string Email, string Code);
        /// <summary>
        /// 取消绑定邮箱 -发送验证码
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        Task<bool> SendVerifyCodeForUnBindEmailAsync(string Email, string Code);
        /// <summary>
        /// 发送通知给管理员，回答社区提问
        /// </summary>
        /// <param name="TopicId"></param>
        /// <returns></returns>
        Task<bool> SendEmailToManagerForAnswerAsync(string TopicId);

        /// <summary>
        /// 发送通知给主贴发帖人，有新的回复
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="TopicTitle"></param>
        /// <param name="TopicId"></param>
        /// <returns></returns>
        Task<bool> SendEmailToTopicCreatorAsync(string Email, string TopicTitle,string TopicId);


    }
}