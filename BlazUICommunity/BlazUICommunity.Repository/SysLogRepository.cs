using Arch.EntityFrameworkCore.UnitOfWork;
using BlazUICommunity.Model.Models;
using System;

namespace BlazUICommunity.Repository
{
    public class SysLogRepository : Repository<SysLogModel>, IRepository<SysLogModel>
    {
        public SysLogRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {

        }
    }
}
