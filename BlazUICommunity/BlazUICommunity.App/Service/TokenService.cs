using Blazui.Community.HttpClientExtensions;
using Blazui.Community.Model.Models;
using Blazui.Community.Response;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Blazui.Community.App.Service
{
    public class TokenService
    {
        private readonly AuthenticationStateProvider _AuthenticationStateProvider;
        private readonly UserManager<BZUserModel> _userManager;
        private readonly HttpClient httpClient;
        private readonly IMemoryCache _memoryCache;

        public TokenService(IHttpClientFactory httpClientFactory, AuthenticationStateProvider authenticationStateProvider, UserManager<BZUserModel> userManager, IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _AuthenticationStateProvider = authenticationStateProvider;
            _userManager = userManager;
            this.httpClient = httpClientFactory.CreateClient("BlazuiCommunitiyApp");
        }

        /// <summary>
        /// 构建HttpContent
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        private static HttpContent BuildHttpContent<T>(T t)
        {
            if (t is null)
                return new StringContent("");
            var requestJson = JsonConvert.SerializeObject(t);
            HttpContent httpContent = new StringContent(requestJson);
            return httpContent;
        }

        private async Task<BaseResponse<Token>> CreateToken(LoginInModel loginmodel)
        {
            return await _memoryCache.GetOrCreateAsync(loginmodel.Username, async p =>
            {
                p.SetSlidingExpiration(TimeSpan.FromMinutes(30));
                HttpContent httpContent = BuildHttpContent(loginmodel);
                return await httpClient.PostWithJsonResultAsync<Token>("token", httpContent);
            });
        }

        public async Task<BaseResponse<Token>> RquestToken()
        {
            if ((await Logged()) is LoginInModel loginInModel)
                return await CreateToken(loginInModel);
            return new BaseResponse<Token>(401, "Unauthorized");
        }

        private async Task<LoginInModel> Logged()
        {
            var userStatue = await _AuthenticationStateProvider.GetAuthenticationStateAsync();
            if (userStatue.User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(userStatue.User);
                return new LoginInModel() { Password = user.PasswordHash, Username = user.UserName };
            }
            return null;
        }
    }

    public class Token
    {
        /// <summary>
        ///
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        ///
        /// </summary>
        public long RefreshTokenExpired { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string RefreshToken { get; set; }
    }

    public class LoginInModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}