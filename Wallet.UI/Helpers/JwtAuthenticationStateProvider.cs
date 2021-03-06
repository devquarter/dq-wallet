using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace Wallet.UI.Helpers
{
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly AuthenticationState _anonymous;
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public JwtAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var tokenString = await _localStorage.GetItemAsStringAsync("authToken");

            if (string.IsNullOrWhiteSpace(tokenString))
            {
                return _anonymous;
            }

            var token = new JwtSecurityToken(tokenString);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", tokenString);

            return new AuthenticationState(new ClaimsPrincipal(GetClaimsIdentity(token)));
        }

        public void NotifyUserAuthentication(string tokenString)
        {
            var token = new JwtSecurityToken(tokenString);
            var authenticatedUser = new ClaimsPrincipal(GetClaimsIdentity(token));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public void NotifyUserLogout() => NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));

        private static ClaimsIdentity GetClaimsIdentity(JwtSecurityToken token) => new (token.Claims, "jwtAuthType", "name", "role");
    }
}