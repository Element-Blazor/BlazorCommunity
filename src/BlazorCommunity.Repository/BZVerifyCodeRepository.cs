using Arch.EntityFrameworkCore.UnitOfWork;
using BlazorCommunity.AppDbContext;
using BlazorCommunity.Model.Models;

namespace BlazorCommunity.Repository
{
    public class BZVerifyCodeRepository : Repository<BzVerifyCodeModel>, IRepository<BzVerifyCodeModel>
    {
        public BZVerifyCodeRepository(BlazorCommunityContext dbContext) : base(dbContext)
        {
        }
    }
}