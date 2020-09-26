using Arch.EntityFrameworkCore.UnitOfWork;
using BlazorCommunity.AppDbContext;
using BlazorCommunity.Model.Models;

namespace BlazorCommunity.Repository
{
    public class BZAddressRepository : Repository<BZAddressModel>, IRepository<BZAddressModel>
    {
        public BZAddressRepository(BlazorCommunityContext dbContext) : base(dbContext)
        {
        }
    }
}