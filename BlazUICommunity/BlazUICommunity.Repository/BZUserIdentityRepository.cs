using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Repository
{
    public class BZUserIdentityRepository : Repository<BZUserModel>, IRepository<BZUserModel>
    {
        private UserManager<BZUserModel> _userManager;
        private IUnitOfWork _IUnitOfWork;
        public BZUserIdentityRepository(BlazUICommunityContext dbContext, UserManager<BZUserModel> userManager, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _IUnitOfWork = unitOfWork;
            _userManager = userManager;
        }


        public async Task<IdentityResult> CreateUserAsync(string userAccount, string Password, string Mobile = null, string Email = null, string NickName = null, int Sex = 0, string CreatorId = null)
        {
            if (string.IsNullOrEmpty(userAccount))
            {
                throw new ArgumentException("message", nameof(userAccount));
            }

            if (string.IsNullOrEmpty(Password))
            {
                throw new ArgumentException("message", nameof(Password));
            }
            return await _userManager.CreateAsync(
                   new BZUserModel
                   {
                       UserName = userAccount,
                       NickName = NickName ?? userAccount,
                       Email = Email ?? "",
                       EmailConfirmed = false,
                       NormalizedUserName = userAccount,
                       CreateDate = DateTime.Now,
                       LastLoginDate = DateTime.Now,
                       CreatorId = CreatorId ?? Guid.Empty.ToString(),
                       Sex = Sex,
                       Status = 0,
                       Avator = "/img/defaultAct.png",
                       PhoneNumber = Mobile ?? ""
                   }, Password);

        }
    }
}
