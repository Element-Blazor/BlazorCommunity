﻿namespace BlazorCommunity.DTO
{
    public class BZPointDto : BaseDto
    {
        /// <summary>
        /// 积分来源，1：发帖，2：回帖，3:精华帖，4：其他--规则
        /// </summary>
        public int? Access { get; set; }

        /// <summary>
        /// 加积分/扣积分
        /// </summary>
        public int? Score { get; set; }

        /// <summary>
        ///  描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }
    }
}