namespace Blazui.Community.DTO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public  class SysLogDto : BaseDto
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
