using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.Model.Models;
using System;

namespace Blazui.Community.Repository
{
    public class SysRoleRepository : Repository<SysRoleModel>, IRepository<SysRoleModel>
    {
        public SysRoleRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {

        }
    }
}
