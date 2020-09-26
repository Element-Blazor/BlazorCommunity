using Arch.EntityFrameworkCore.UnitOfWork;
using BlazorCommunity.AppDbContext;
using BlazorCommunity.Model.Models;
using Microsoft.AspNetCore.Identity;

namespace BlazorCommunity.Repository
{
    public class BZUserRoleRepository : Repository<IdentityUserRole<string>>, IRepository<IdentityUserRole<string>>
    {
        public BZUserRoleRepository(BlazorCommunityContext dbContext) : base(dbContext)
        {

        }
    }
}