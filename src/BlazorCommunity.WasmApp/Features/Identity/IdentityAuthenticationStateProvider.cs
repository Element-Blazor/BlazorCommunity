//using BlazorCommunity.Model.Models;
//using BlazorCommunity.Shared;
//using BlazorCommunity.WasmApp.Service;
//using Microsoft.AspNetCore.Components.Authorization;
//using System;
//using System.Linq;
//using System.Net.Http;
//using System.Security.Claims;
//using System.Threading.Tasks;

//namespace BlazorCommunity.WasmApp.Features.Identity
//{
//    public class IdentityAuthenticationStateProvider : AuthenticationStateProvider
//    {
//        private UserInfo _userInfoCache;
//        private readonly IAuthenticationService authorizeService;

//        public IdentityAuthenticationStateProvider(IAuthenticationService authorizeService)
//        {
//            this.authorizeService = authorizeService;
//        }

//        public async Task<LoginResult> Login(LoginModel loginParameters)
//        {
//            var result= await authorizeService.Login(loginParameters);
//            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
//            return result;
//        }

//        public async Task<RegisterResult> Register(RegisterModel registerParameters)
//        {
//           var result= await authorizeService.Register(registerParameters);
//            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
//            return result;
//        }

//        public async Task Logout()
//        {
//            await authorizeService.Logout();
//            _userInfoCache = null;
//            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
//        }

//        public async Task<UserInfo> GetUserInfo()
//        {
//            if (_userInfoCache != null && _userInfoCache.IsAuthenticated) return _userInfoCache;
//            _userInfoCache = await authorizeService.GetUserInfo();
//            return _userInfoCache;
//        }

//        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
//        {
//            var identity = new ClaimsIdentity();
//            try
//            {
//                var userInfo = await GetUserInfo();
//                if (userInfo.IsAuthenticated)
//                {
//                    var claims = new[] { new Claim(ClaimTypes.Name, _userInfoCache.UserName),new Claim("Avator",_userInfoCache.UserAvator) }
//                    .Concat(_userInfoCache.ExposedClaims.Select(c => new Claim(c.Key, c.Value)));
//                    identity = new ClaimsIdentity(claims, "Server authentication");
//                }
//            }
//            catch (HttpRequestException ex)
//            {
//                Console.WriteLine("Request failed:" + ex.ToString());
//            }

//            return new AuthenticationState(new ClaimsPrincipal(identity));
//        }
//    }
//}
