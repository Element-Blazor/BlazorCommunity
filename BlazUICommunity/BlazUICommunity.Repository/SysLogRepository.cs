using Arch.EntityFrameworkCore.UnitOfWork;
using BlazUICommunity.Model.Models;
using System;

namespace BlazUICommunity.Repository
{
    public class SysLogRepository : Repository<SysLog>, IRepository<SysLog>
    {
        public SysLogRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {

        }
    }
}
