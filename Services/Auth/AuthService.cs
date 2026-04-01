using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;
using MOBILE.SIGE.Interfaces;
using MOBILE.SIGE.Models.Login;
using MOBILE.SIGE.Services.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MOBILE.SIGE.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;
        private readonly TokenStorageService _tokenStorage;
        private readonly AuthenticationStateProvider _authStateProvider;

        // TEMPORARIO: usuario local para testes - REMOVER ANTES DE PRODUÇÃO
        private const string TempEmail = "admin@sige.local";
        private const string TempSenha = "admin123";

        public AuthService(HttpClient http, TokenStorageService tokenStorage, AuthenticationStateProvider authStateProvider)
        {
            _http = http;
            _tokenStorage = tokenStorage;
            _authStateProvider = authStateProvider;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            // TEMPORARIO: bypass local para desenvolvimento
            if (request.Email == TempEmail && request.Senha == TempSenha)
            {
                var token = GerarTokenTemporario();
                await _tokenStorage.SetTokenAsync(token);
                ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(token);

                return new LoginResponse
                {
                    Success = true,
                    IdUsuario = 1,
                    NomeUsuario = "Admin Temp",
                    Email = TempEmail,
                    TipoUsuario = 1,
                    Cargos = new List<string> { "Administrador" },
                    Token = token,
                    Message = "Login local temporario."
                };
            }

            var response = await _http.PostAsJsonAsync("api/UsuarioApi/login", request);
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>()
                ?? new LoginResponse { Success = false, Message = "Erro ao processar resposta." };

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

        private static string GerarTokenTemporario()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ChaveTemporariaLocalSIGE2026DevOnly!!"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, "Admin Temp"),
                new Claim(ClaimTypes.Email, TempEmail),
                new Claim(ClaimTypes.Role, "Administrador"),
                new Claim("idUsuario", "1"),
                new Claim("tipoUsuario", "1")
            };

            var token = new JwtSecurityToken(
                issuer: "SIGE.Local",
                audience: "SIGE.Web",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
