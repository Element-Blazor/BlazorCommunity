﻿namespace BlazorCommunity.DTO
{
    public class BZOauthDto : BaseDto
    {
        /// <summary>
        /// 登录类型：0：QQ，1：微信，2：微博，...
        /// </summary>
        public int OauthType { get; set; }

        /// <summary>
        /// OauthName
        /// </summary>
        public string OauthName { get; set; }

        /// <summary>
        /// OauthId
        /// </summary>
        public string OauthId { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string HomePage { get; set; }
    }
}