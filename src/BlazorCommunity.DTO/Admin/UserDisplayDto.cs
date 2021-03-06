﻿using BlazorCommunity.AutoMapperExtensions;
using BlazorCommunity.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorCommunity.DTO.Admin
{
    public class UserDisplayDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 0-男，1-女
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [AutoNotMap]
        public string SexDisplay => ((Sex)this.Sex).Description();

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [AutoNotMap]
        public string StatusDisplay => ((DelStatus)this.Status).Description();

        /// <summary>
        /// 签名
        /// </summary>
        [NotMapped]
        public string Signature { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        [NotMapped]
        public string RoleName { get; set; }
    }
}