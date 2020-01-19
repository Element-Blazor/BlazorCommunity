using Arch.EntityFrameworkCore.UnitOfWork;
using BlazUICommunity.Model.Models;
using System;

namespace BlazUICommunity.Repository
{
    public class BZTopicRepository : Repository<BZTopicModel>, IRepository<BZTopicModel>
    {
        public BZTopicRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {

        }
    }
}
