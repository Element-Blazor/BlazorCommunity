using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.Model.Models;
using System;

namespace Blazui.Community.Repository
{
    public class BZAddressRepository : Repository<BZAddressModel>, IRepository<BZAddressModel>
    {
        public BZAddressRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {

        }
    }
}
