using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.Model.Models;
using System;

namespace Blazui.Community.Repository
{
    public class BZVerifyCodeRepository : Repository<BzVerifyCodeModel>, IRepository<BzVerifyCodeModel>
    {
        public BZVerifyCodeRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {

        }
    }
}
