using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazui.Community.Repository
{
    public class BZReplyRepository : Repository<BZReplyModel>, IRepository<BZReplyModel>
    {
        public BZReplyRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {
        }

       
    }
}