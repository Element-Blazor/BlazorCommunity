using Blazui.Community.Response;
using Blazui.Community.Shared;
using System.Threading.Tasks;

namespace Blazui.Community.WasmApp.Service
{
    public interface IAuthenticationService
    {
        Task<LoginResult> Login(LoginModel loginModel);
        Task Logout();
        Task<RegisterResult> Register(RegisterModel registerModel);

    }
}
