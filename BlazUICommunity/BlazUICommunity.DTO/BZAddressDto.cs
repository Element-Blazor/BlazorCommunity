using System;
using System.ComponentModel.DataAnnotations;

namespace Blazui.Community.DTO
{
    public class BZAddressDto
    {

        /// <summary>
        /// 国家
        /// </summary>
        [StringLength(20)]
        public string Country { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        [StringLength(20)]
        public string Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        [StringLength(20)]
        public string City { get; set; }
        /// <summary>
        /// 地区
        /// </summary>
        [StringLength(20)]
        public string District { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }
    }
}
