using Arch.EntityFrameworkCore.UnitOfWork;
using BlazUICommunity.Model.Models;
using System;

namespace BlazUICommunity.Repository
{
    public class SysMenuRepository : Repository<SysMenuModel>, IRepository<SysMenuModel>
    {
        public SysMenuRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {

        }
    }
}
