using Microsoft.AspNetCore.Components.Authorization;
using MOBILE.SIGE.Interfaces;
using MOBILE.SIGE.Models.Login;
using MOBILE.SIGE.Services.Auth;
using System.Text.Json;

namespace MOBILE.SIGE.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;
        private readonly TokenStorageService _tokenStorage;
        private readonly AuthenticationStateProvider _authStateProvider;

        public AuthService(IHttpClientFactory factory, TokenStorageService tokenStorage, AuthenticationStateProvider authStateProvider)
        {
            _http = factory.CreateClient("ApiSige");
            _tokenStorage = tokenStorage;
            _authStateProvider = authStateProvider;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var response = await _http.PostAsJsonAsync("api/usuario/login", request);
            var content = await response.Content.ReadAsStringAsync();
            LoginResponse? result = null;

            if (!string.IsNullOrWhiteSpace(content))
            {
                try
                {
                    result = JsonSerializer.Deserialize<LoginResponse>(
                        content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                catch
                {
                }
            }

            if (result is null)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = !response.IsSuccessStatusCode
                        ? $"Falha no login (HTTP {(int)response.StatusCode}) [{response.RequestMessage?.RequestUri}]: {content}"
                        : "A API retornou uma resposta inválida."
                };
            }

            if (result.Success && result.Token != null)
            {
                await _tokenStorage.SetTokenAsync(result.Token);
                ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.Token);
            }

            return result;
        }

        public async Task LogoutAsync()
        {
            await _tokenStorage.RemoveTokenAsync();
            ((CustomAuthStateProvider)_authStateProvider).NotifyUserLogout();
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await _tokenStorage.GetTokenAsync();
            return !string.IsNullOrWhiteSpace(token);
        }
    }
}
