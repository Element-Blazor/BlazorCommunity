using Arch.EntityFrameworkCore.UnitOfWork;
using BlazorCommunity.AppDbContext;
using BlazorCommunity.Model.Models;

namespace BlazorCommunity.Repository
{
    public class BZBannerRepository : Repository<BzBannerModel>, IRepository<BzBannerModel>
    {
        public BZBannerRepository(BlazorCommunityContext dbContext) : base(dbContext)
        {
        }
    }
}