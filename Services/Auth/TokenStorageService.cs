using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace MOBILE.SIGE.Services.Auth
{
    public class TokenStorageService
    {
        private readonly ProtectedLocalStorage _localStorage;
        private const string TokenKey = "authToken";

        public string? CachedToken { get; private set; }

        public TokenStorageService(ProtectedLocalStorage localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task<string?> GetTokenAsync()
        {
            try
            {
                var result = await _localStorage.GetAsync<string>(TokenKey);
                CachedToken = result.Success ? result.Value : null;
                return CachedToken;
            }
            catch
            {
                return CachedToken;
            }
        }

        public async Task SetTokenAsync(string token)
        {
            CachedToken = token;
            try
            {
                await _localStorage.SetAsync(TokenKey, token);
            }
            catch (InvalidOperationException)
            {
            }
        }

        public async Task RemoveTokenAsync()
        {
            CachedToken = null;
            try
            {
                await _localStorage.DeleteAsync(TokenKey);
            }
            catch (InvalidOperationException)
            {
            }
        }
    }
}
