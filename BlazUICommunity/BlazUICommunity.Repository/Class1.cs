using Arch.EntityFrameworkCore.UnitOfWork;
using BlazUICommunity.Model.Models;
using System;

namespace BlazUICommunity.Repository
{
    public class UserRepository : Repository<BZUserModel>, IRepository<BZUserModel>
    {
        public UserRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {

        }
    }
}
