using Arch.EntityFrameworkCore.UnitOfWork;
using BlazorCommunity.AppDbContext;
using BlazorCommunity.Model.Models;

namespace BlazorCommunity.Repository
{
    public class BZUserRealVerificationRepository : Repository<BZIDCardModel>, IRepository<BZIDCardModel>
    {
        public BZUserRealVerificationRepository(BlazorCommunityContext dbContext) : base(dbContext)
        {
        }
    }
}