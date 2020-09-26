using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.AppDbContext;
using Blazui.Community.Model.Models;

namespace Blazui.Community.Repository
{
    public class BZAuthoRepository : Repository<BZAutho2Model>, IRepository<BZAutho2Model>
    {
        public BZAuthoRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {
        }
    }
}