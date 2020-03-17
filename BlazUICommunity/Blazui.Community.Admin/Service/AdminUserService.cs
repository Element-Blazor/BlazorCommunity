using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Admin.Service
{
    public class AdminUserService : AdminUserServiceBase<IdentityUser, IdentityRole>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AuthenticationStateProvider _AuthenticationStateProvider;

        public AdminUserService(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            AuthenticationStateProvider AuthenticationStateProvider) : base(signInManager, roleManager)
        {
            this._userManager = userManager;
            _AuthenticationStateProvider = AuthenticationStateProvider;
        }

        public override async Task<string> CreateRoleAsync(string roleName, string id)
        {
            var role = new IdentityRole(roleName);
            role.Id = id;
            var result = await RoleManager.CreateAsync(role);
            return GetResultMessage(result);
        }

        public override async Task<string> CreateUserAsync(string username, string password)
        {
            var user = new IdentityUser(username);
            var result = await SignInManager.UserManager.CreateAsync(user, password);
            return GetResultMessage(result);
        }

        public override async Task<string> DeleteUsersAsync(params object[] users)
        {
            foreach (IdentityUser item in users)
            {
                var result = await SignInManager.UserManager.DeleteAsync(item);
                if (result.Succeeded)
                {
                    continue;
                }
                return GetResultMessage(result);
            }
            return string.Empty;
        }

        public async Task<bool> IsSupperAdmin()
        {
            return (await _userManager.GetRolesAsync(
                         await _userManager.GetUserAsync(
                        (await _AuthenticationStateProvider.GetAuthenticationStateAsync()).User))
                ).Any(p => p == "管理员");
        }
    }
}