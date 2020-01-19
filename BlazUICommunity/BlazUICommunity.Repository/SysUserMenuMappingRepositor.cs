using Arch.EntityFrameworkCore.UnitOfWork;
using BlazUICommunity.Model.Models;
using System;

namespace BlazUICommunity.Repository
{
    public class SysUserMenuMappingRepositor : Repository<SysUserMenuMappingModel>, IRepository<SysUserMenuMappingModel>
    {
        public SysUserMenuMappingRepositor(BlazUICommunityContext dbContext) : base(dbContext)
        {

        }
    }
}
