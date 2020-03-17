namespace Blazui.Community.DTO
{
    public class SysLogDto : BaseDto
    {
        public string UserName { get; set; }

        /// <summary>
        /// ��Ҫ
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public string Detail { get; set; }

        public byte LogType { get; set; }
    }
}