using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.Model.Models;
using System;
using System.Threading.Tasks;

namespace Blazui.Community.Repository
{
    public class BZFollowRepository : Repository<BZFollowModel>, IRepository<BZFollowModel>
    {
        public BZFollowRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {


        }

        public async Task<bool> Cancel(int TopicId,int UserId)
        {
            string sql = $"update follow set `Status`=-1 where TopicId={TopicId} and UserId={UserId}";

            return await ExecuteSqlCmd(sql);
        }
    }
}
