using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace MOBILE.SIGE.Services.Auth
{
    public class TokenStorageService
    {
        private readonly ProtectedLocalStorage _localStorage;
        private const string TokenKey = "authToken";

        public TokenStorageService(ProtectedLocalStorage localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task<string?> GetTokenAsync()
        {
            try
            {
                var result = await _localStorage.GetAsync<string>(TokenKey);
                return result.Success ? result.Value : null;
            }
            catch
            {
                return null;
            }
        }

        public async Task SetTokenAsync(string token)
        {
            await _localStorage.SetAsync(TokenKey, token);
        }

        public async Task RemoveTokenAsync()
        {
            await _localStorage.DeleteAsync(TokenKey);
        }
    }
}
