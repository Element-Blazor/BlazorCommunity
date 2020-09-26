using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.AppDbContext;
using Blazui.Community.Model.Models;

namespace Blazui.Community.Repository
{
    public class BZAddressRepository : Repository<BZAddressModel>, IRepository<BZAddressModel>
    {
        public BZAddressRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {
        }
    }
}