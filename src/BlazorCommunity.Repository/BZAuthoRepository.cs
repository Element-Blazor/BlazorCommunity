using Arch.EntityFrameworkCore.UnitOfWork;
using BlazorCommunity.AppDbContext;
using BlazorCommunity.Model.Models;

namespace BlazorCommunity.Repository
{
    public class BZAuthoRepository : Repository<BZAutho2Model>, IRepository<BZAutho2Model>
    {
        public BZAuthoRepository(BlazorCommunityContext dbContext) : base(dbContext)
        {
        }
    }
}