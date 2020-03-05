namespace Blazui.Community.DTO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public  class SysUserMenuDto : BaseDto
    {

        public string SysUserId { get; set; }

        public string SysMenuId { get; set; }
    }
}
