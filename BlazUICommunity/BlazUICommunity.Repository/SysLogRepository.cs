using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.Model.Models;
using System;

namespace Blazui.Community.Repository
{
    public class SysLogRepository : Repository<SysLogModel>, IRepository<SysLogModel>
    {
        public SysLogRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {

        }
    }
}
