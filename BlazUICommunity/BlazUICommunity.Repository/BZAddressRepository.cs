using Arch.EntityFrameworkCore.UnitOfWork;
using BlazUICommunity.Model.Models;
using System;

namespace BlazUICommunity.Repository
{
    public class BZAddressRepository : Repository<BZAddressModel>, IRepository<BZAddressModel>
    {
        public BZAddressRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {

        }
    }
}
