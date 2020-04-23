using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.Model.Models;
using Microsoft.AspNetCore.Identity;

namespace Blazui.Community.Repository
{
    public class BZUserRoleRepository : Repository<IdentityUserRole<string>>, IRepository<IdentityUserRole<string>>
    {
        public BZUserRoleRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {

        }
    }
}