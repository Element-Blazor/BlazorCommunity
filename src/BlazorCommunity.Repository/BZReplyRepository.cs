using Arch.EntityFrameworkCore.UnitOfWork;
using BlazorCommunity.AppDbContext;
using BlazorCommunity.Model.Models;

namespace BlazorCommunity.Repository
{
    public class BZReplyRepository : Repository<BZReplyModel>, IRepository<BZReplyModel>
    {
        public BZReplyRepository(BlazorCommunityContext dbContext) : base(dbContext)
        {
        }
    }
}