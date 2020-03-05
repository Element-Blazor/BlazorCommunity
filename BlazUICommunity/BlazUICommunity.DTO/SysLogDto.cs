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
