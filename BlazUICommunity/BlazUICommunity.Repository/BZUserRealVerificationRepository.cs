using Arch.EntityFrameworkCore.UnitOfWork;
using BlazUICommunity.Model.Models;
using System;

namespace BlazUICommunity.Repository
{
    public class BZUserRealVerificationRepository : Repository<BZUserRealVerificationModel>, IRepository<BZUserRealVerificationModel>
    {
        public BZUserRealVerificationRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {

        }
    }
}
