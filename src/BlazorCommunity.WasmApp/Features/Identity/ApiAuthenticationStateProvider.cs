using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using static BlazorCommunity.WasmApp.ConstConfig;
namespace BlazorCommunity.WasmApp.Features.Identity
{
    public class ApiAuthenticationStateProvider
        : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService localStorage;

        public ApiAuthenticationStateProvider(IHttpClientFactory  httpClientFactory, ILocalStorageService localStorage)
        {
            _httpClient = httpClientFactory.CreateClient(HttpClientBaseName);
            this.localStorage = localStorage;
        }
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var savedTokenJson = await localStorage.GetItemAsync<string>("authToken");
                if (string.IsNullOrWhiteSpace(savedTokenJson))
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                var RefreshTokenExpired = await localStorage.GetItemAsync<string>("RefreshTokenExpired");
                if (DateTime.Now > Convert.ToDateTime(RefreshTokenExpired))
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", savedTokenJson);

                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(savedTokenJson), "jwt")));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }
        public async Task MarkUserAsAuthenticated(string token)
        {
            var claims = ParseClaimsFromJwt(token);
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
            if (authenticatedUser.FindFirst("id") is Claim claim)
               await localStorage.SetItemAsync("CurrentUserId", claim.Value);
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public void MarkUserAsLoggedOut()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));
            NotifyAuthenticationStateChanged(authState);
        }
        private IEnumerable<Claim> ParseClaimsFromJwt(string savedToken)
        {
            var claims = new List<Claim>();
            var payload = savedToken.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

            if (roles != null)
            {
                if (roles.ToString().Trim().StartsWith("["))
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                }

                keyValuePairs.Remove(ClaimTypes.Role);
            }

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;
        }
        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}