namespace Blazui.Community.DTO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public  class SysMenuDto : BaseDto
    {

        public string ParentId { get; set; }

        public string Text { get; set; }

        public string Url { get; set; }

        public byte MenuLevel { get; set; }

        public byte MenuType { get; set; }

        public string MenuIcon { get; set; }

        public string Description { get; set; }

        public string SourcePath { get; set; }

        public int Sort { get; set; }




    }
}
