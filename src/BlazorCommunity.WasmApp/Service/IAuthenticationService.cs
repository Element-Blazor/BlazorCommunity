using BlazorCommunity.Response;
using BlazorCommunity.Shared;
using System.Threading.Tasks;

namespace BlazorCommunity.WasmApp.Service
{
    public interface IAuthenticationService
    {
        Task<LoginResult> Login(LoginModel loginModel);
        Task Logout();
        Task<RegisterResult> Register(RegisterModel registerModel);

    }
}
