using Arch.EntityFrameworkCore.UnitOfWork;
using BlazUICommunity.Model.Models;
using System;

namespace BlazUICommunity.Repository
{
    public class BZPointRepository : Repository<BZPointModel>, IRepository<BZPointModel>
    {
        public BZPointRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {

        }
    }
}
