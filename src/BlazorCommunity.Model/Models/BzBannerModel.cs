using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorCommunity.Model.Models
{
    [Table("BZBanner")]
    public class BzBannerModel : BaseModel
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
        /// 链接
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool Show { get; set; }
    }
}