using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.AppDbContext;
using Blazui.Community.Model.Models;

namespace Blazui.Community.Repository
{
    public class BZPointRepository : Repository<BZPointModel>, IRepository<BZPointModel>
    {
        public BZPointRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {
        }
    }
}