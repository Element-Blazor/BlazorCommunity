using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.Model.Models;

namespace Blazui.Community.Repository
{
    public class SysUserMenuMappingRepositor : Repository<SysUserMenuMappingModel>, IRepository<SysUserMenuMappingModel>
    {
        public SysUserMenuMappingRepositor(BlazUICommunityContext dbContext) : base(dbContext)
        {
        }
    }
}