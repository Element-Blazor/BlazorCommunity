using Blazui.Community.AutoMapperExtensions;
using Blazui.Community.DateTimeExtensions;
using Blazui.Community.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazui.Community.DTO.Admin
{
    public class BannerDisplayDto
    {
        /// <summary>
        /// banner标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// bannerImg
        /// </summary>
        public string BannerImg { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool Show { get; set; }

        public string Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime LastModifyDate { get; set; }

        /// <summary>
        /// 创建人Id
        /// </summary>
        public string CreatorId { get; set; }

        /// <summary>
        /// 最后修改人Id
        /// </summary>
        public string LastModifierId { get; set; }

        /// <summary>
        /// 删除状态 0：正常，-1：已删除
        /// </summary>

        public int Status { get; set; } = 0;

        [AutoNotMap]
        [NotMapped]
        public string StatusDisplay => ((DelStatus)Status).Description();

        [AutoNotMap]
        [NotMapped]
        public string CreateDateDisplay => CreateDate.ConvertToDateDiffStr();

        /// <summary>
        /// 为了不依赖blazui组件 使用object代替IFileModel
        /// </summary>
        [AutoNotMap]
        [NotMapped]
        public object[] Previews { get; set; }
    }
}