using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.Model.Models;

namespace Blazui.Community.Repository
{
    public class SysMenuRepository : Repository<SysMenuModel>, IRepository<SysMenuModel>
    {
        public SysMenuRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {
        }
    }
}