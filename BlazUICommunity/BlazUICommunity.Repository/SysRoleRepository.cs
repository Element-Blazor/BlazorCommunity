using Arch.EntityFrameworkCore.UnitOfWork;
using BlazUICommunity.Model.Models;
using System;

namespace BlazUICommunity.Repository
{
    public class SysRoleRepository : Repository<SysRoleModel>, IRepository<SysRoleModel>
    {
        public SysRoleRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {

        }
    }
}
