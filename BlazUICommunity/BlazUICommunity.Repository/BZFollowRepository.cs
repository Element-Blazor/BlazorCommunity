using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.Model.Models;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace Blazui.Community.Repository
{
    public class BZFollowRepository : Repository<BZFollowModel>, IRepository<BZFollowModel>
    {
        public BZFollowRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {
        }
    }
}