using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.AppDbContext;
using Blazui.Community.Model.Models;

namespace Blazui.Community.Repository
{
    public class BZBannerRepository : Repository<BzBannerModel>, IRepository<BzBannerModel>
    {
        public BZBannerRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {
        }
    }
}