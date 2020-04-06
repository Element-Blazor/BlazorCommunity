using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.Model.Models;

namespace Blazui.Community.Repository
{
    public class BZReplyRepository : Repository<BZReplyModel>, IRepository<BZReplyModel>
    {
        public BZReplyRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {
        }
    }
}