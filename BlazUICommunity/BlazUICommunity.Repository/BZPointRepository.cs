using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.Model.Models;
using System;

namespace Blazui.Community.Repository
{
    public class BZPointRepository : Repository<BZPointModel>, IRepository<BZPointModel>
    {
        public BZPointRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {

        }
    }
}
