using Blazui.Community.WebassemblyApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.WebassemblyApp.Client.Services.Contracts
{
    public interface IAuthorizeApi
    {
        Task Login(LoginParameters loginParameters);
        Task Register(RegisterParameters registerParameters);
        Task Logout();
        Task<UserInfo> GetUserInfo();
    }
}
