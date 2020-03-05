using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.Model.Models;
using System;

namespace Blazui.Community.Repository
{
    public class BZBannerRepository : Repository<BzBannerModel>, IRepository<BzBannerModel>
    {
        public BZBannerRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {

        }
    }
}
