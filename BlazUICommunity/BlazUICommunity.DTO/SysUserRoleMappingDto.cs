namespace Blazui.Community.DTO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public  class SysUserRoleMappingDto : BaseDto
    {

        public string SysUserId { get; set; }

        public string SysRoleId { get; set; }
    }
}
