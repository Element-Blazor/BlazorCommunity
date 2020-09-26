namespace Blazui.Community.DTO
{
    public class BzBannerDto : BaseDto
    {
        /// <summary>
        /// banner标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// bannerImg
        /// </summary>
        public string BannerImg { get; set; }

        public string Link { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool Show { get; set; }
    }
}