using Arch.EntityFrameworkCore.UnitOfWork;
using BlazUICommunity.Model.Models;
using System;

namespace BlazUICommunity.Repository
{
    public class SysRoleMenuMappingRepository : Repository<SysRoleMenuMappingModel>, IRepository<SysRoleMenuMappingModel>
    {
        public SysRoleMenuMappingRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {

        }
    }
}
