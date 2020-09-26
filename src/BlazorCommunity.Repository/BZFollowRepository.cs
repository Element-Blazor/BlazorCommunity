using Arch.EntityFrameworkCore.UnitOfWork;
using BlazorCommunity.AppDbContext;
using BlazorCommunity.Model.Models;

namespace BlazorCommunity.Repository
{
    public class BZFollowRepository : Repository<BZFollowModel>, IRepository<BZFollowModel>
    {
        public BZFollowRepository(BlazorCommunityContext dbContext) : base(dbContext)
        {
        }
    }
}