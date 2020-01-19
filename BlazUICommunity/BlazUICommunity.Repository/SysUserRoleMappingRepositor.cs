using Arch.EntityFrameworkCore.UnitOfWork;
using BlazUICommunity.Model.Models;
using System;

namespace BlazUICommunity.Repository
{
    public class SysUserRoleMappingRepositor : Repository<SysUserRoleMappingModel>, IRepository<SysUserRoleMappingModel>
    {
        public SysUserRoleMappingRepositor(BlazUICommunityContext dbContext) : base(dbContext)
        {

        }
    }
}
