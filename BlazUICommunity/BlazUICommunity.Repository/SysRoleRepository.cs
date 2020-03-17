using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.Model.Models;

namespace Blazui.Community.Repository
{
    public class SysRoleRepository : Repository<SysRoleModel>, IRepository<SysRoleModel>
    {
        public SysRoleRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {
        }
    }
}