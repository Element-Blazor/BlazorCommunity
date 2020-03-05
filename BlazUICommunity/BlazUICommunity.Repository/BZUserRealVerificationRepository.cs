using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.Model.Models;
using System;

namespace Blazui.Community.Repository
{
    public class BZUserRealVerificationRepository : Repository<BZIDCardModel>, IRepository<BZIDCardModel>
    {
        public BZUserRealVerificationRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {

        }
    }
}
