using BlazorCommunity.WasmApp.Shared;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorCommunity.Response;
using BlazorCommunity.Shared;
using BlazorCommunity.WasmApp.Features.Identity;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using BlazorCommunity.HttpClientExtensions;

namespace BlazorCommunity.WasmApp.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthenticationService(IHttpClientFactory httpClientFactory,
            AuthenticationStateProvider authenticationStateProvider,
            ILocalStorageService localStorage)
        {
            _httpClient = httpClientFactory.CreateClient("BlazuiCommunitiyApp");
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
        }

        public async Task<RegisterResult> Register(RegisterModel registerModel)
        {
            var unKownErrors = new RegisterResult() { Successful = false, Errors = new List<string> { "注册失败，未知错误" } };
            var result = await _httpClient.PostWithJsonResultAsync<RegisterResult>("api/Account/Regist", registerModel.BuildHttpContent());
            return result?.Data ?? unKownErrors;
        }

        public async Task<LoginResult> Login(LoginModel loginModel)
        {
            var loginAsJson = JsonConvert.SerializeObject(loginModel);
            var response = await _httpClient.PostAsync("api/Account/Login", new StringContent(loginAsJson, Encoding.UTF8, "application/json"));
            var json = await response.Content.ReadAsStringAsync();
            var ResponseResult = JsonConvert.DeserializeObject<BaseResponse<LoginResult>>(json);
            if (!response.IsSuccessStatusCode || !ResponseResult.IsSuccess||!ResponseResult.Data.Successful)
            {
                return ResponseResult.Data;
            }
            var tokenResult = ResponseResult.Data.Token;
            await _localStorage.SetItemAsync("authToken", tokenResult.AccessToken);
            await _localStorage.SetItemAsync("RefreshTokenExpired", tokenResult.RefreshTokenExpired.ToString("yyyy-MM-dd HH:mm:ss"));
            await ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(tokenResult.AccessToken);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", tokenResult.AccessToken);
            return ResponseResult.Data;
        }

        public async Task Logout()
        {
            //await _localStorage.RemoveItemAsync("authToken");
            //await _localStorage.RemoveItemAsync("CurrentUserId");
            //await _localStorage.RemoveItemAsync("CurrentUser");
            await _localStorage.ClearAsync();
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
