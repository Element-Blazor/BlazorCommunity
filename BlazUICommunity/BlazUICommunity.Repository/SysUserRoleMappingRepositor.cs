using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.Model.Models;
using System;

namespace Blazui.Community.Repository
{
    public class SysUserRoleMappingRepositor : Repository<SysUserRoleMappingModel>, IRepository<SysUserRoleMappingModel>
    {
        public SysUserRoleMappingRepositor(BlazUICommunityContext dbContext) : base(dbContext)
        {

        }
    }
}
