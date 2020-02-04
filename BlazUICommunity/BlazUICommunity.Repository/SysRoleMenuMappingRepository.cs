using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.Model.Models;
using System;

namespace Blazui.Community.Repository
{
    public class SysRoleMenuMappingRepository : Repository<SysRoleMenuMappingModel>, IRepository<SysRoleMenuMappingModel>
    {
        public SysRoleMenuMappingRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {

        }
    }
}
