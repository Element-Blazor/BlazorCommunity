using Arch.EntityFrameworkCore.UnitOfWork;
using BlazUICommunity.Model.Models;
using System;

namespace BlazUICommunity.Repository
{
    public class BZFollowRepository : Repository<BZFollowModel>, IRepository<BZFollowModel>
    {
        public BZFollowRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {

        }
    }
}
