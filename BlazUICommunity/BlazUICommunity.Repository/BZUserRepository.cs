using Arch.EntityFrameworkCore.UnitOfWork;
using BlazUICommunity.Model.Models;
using System;

namespace BlazUICommunity.Repository
{
    public class BZUserRepository : Repository<BZUserModel>, IRepository<BZUserModel>
    {
        public BZUserRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {

        }
    }
}
