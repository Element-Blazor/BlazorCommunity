using Arch.EntityFrameworkCore.UnitOfWork;
using BlazUICommunity.Model.Models;
using System;

namespace BlazUICommunity.Repository
{
    public class BZThirdAccountRepository : Repository<BZThirdAccountModel>, IRepository<BZThirdAccountModel>
    {
        public BZThirdAccountRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {

        }
    }
}
