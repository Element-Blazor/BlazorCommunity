using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.Model.Models;
using System;

namespace Blazui.Community.Repository
{
    public class SysMenuRepository : Repository<SysMenuModel>, IRepository<SysMenuModel>
    {
        public SysMenuRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {

        }
    }
}
