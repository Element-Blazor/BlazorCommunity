using Arch.EntityFrameworkCore.UnitOfWork;
using BlazorCommunity.AppDbContext;
using BlazorCommunity.Model.Models;

namespace BlazorCommunity.Repository
{
    public class BZPointRepository : Repository<BZPointModel>, IRepository<BZPointModel>
    {
        public BZPointRepository(BlazorCommunityContext dbContext) : base(dbContext)
        {
        }
    }
}