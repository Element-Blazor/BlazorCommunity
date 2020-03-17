namespace Blazui.Community.DTO
{
    public class SysLogDto : BaseDto
    {
        public string UserName { get; set; }

        /// <summary>
        /// ธลาช
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// ฯ๊ว้
        /// </summary>
        public string Detail { get; set; }

        public byte LogType { get; set; }
    }
}