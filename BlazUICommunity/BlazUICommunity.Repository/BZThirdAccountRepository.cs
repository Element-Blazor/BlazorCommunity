using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.Model.Models;
using System;

namespace Blazui.Community.Repository
{
    public class BZThirdAccountRepository : Repository<BZThirdAccountModel>, IRepository<BZThirdAccountModel>
    {
        public BZThirdAccountRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {

        }
    }
}
