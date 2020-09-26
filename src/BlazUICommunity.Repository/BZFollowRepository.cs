using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.AppDbContext;
using Blazui.Community.Model.Models;

namespace Blazui.Community.Repository
{
    public class BZFollowRepository : Repository<BZFollowModel>, IRepository<BZFollowModel>
    {
        public BZFollowRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {
        }
    }
}