using Blazui.Community.Admin.QueryCondition;
using Blazui.Community.DTO;
using Blazui.Community.Enums;
using Blazui.Community.Utility;
using Blazui.Component;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazui.Community.Admin.ViewModel
{
    public class BannerAutoGenerateColumnsDto
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
        //[Required]
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? LastModifyDate { get; set; }
        /// <summary>
        /// 创建人Id
        /// </summary>
        //[Required]
        public string CreatorId { get; set; }
        /// <summary>
        /// 删除状态 0：正常，-1：已删除
        /// </summary>

        public int Status { get; set; } = 0;
        [NotMapped]
        public string StatusDisplay
        {
            get
            {
                return ((DelStatus)Status).Description();
            }
        }
        [NotMapped]
        public string CreateDateDisplay
        {
            get
            {
                return CreateDate.ConvertToDateDiffStr();
            }
        }
        /// <summary>
        /// 最后修改人Id
        /// </summary>
        public string LastModifierId { get; set; } = null;

        [NotMapped]
        public IFileModel[] Previews { get; set; }
    }

}
