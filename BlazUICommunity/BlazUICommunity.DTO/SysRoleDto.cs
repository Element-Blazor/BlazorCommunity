namespace Blazui.Community.DTO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public  class SysRoleDto : BaseDto
    {

        public string Text { get; set; }

        public string Description { get; set; }


    }
}
