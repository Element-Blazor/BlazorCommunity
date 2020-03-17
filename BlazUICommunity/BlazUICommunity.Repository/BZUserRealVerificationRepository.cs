using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.Model.Models;

namespace Blazui.Community.Repository
{
    public class BZUserRealVerificationRepository : Repository<BZIDCardModel>, IRepository<BZIDCardModel>
    {
        public BZUserRealVerificationRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {
        }
    }
}