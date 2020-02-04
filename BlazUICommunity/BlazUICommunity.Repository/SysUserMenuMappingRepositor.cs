using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.Model.Models;
using System;

namespace Blazui.Community.Repository
{
    public class SysUserMenuMappingRepositor : Repository<SysUserMenuMappingModel>, IRepository<SysUserMenuMappingModel>
    {
        public SysUserMenuMappingRepositor(BlazUICommunityContext dbContext) : base(dbContext)
        {

        }
    }
}
