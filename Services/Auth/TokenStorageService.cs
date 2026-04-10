using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace MOBILE.SIGE.Services.Auth;

public class TokenStorageService
{
    private readonly ProtectedLocalStorage _storage;
    private const string TokenKey = "authToken";

    public string? CachedToken { get; private set; }

    public TokenStorageService(ProtectedLocalStorage storage)
    {
        _storage = storage;
    }

    public async Task<string?> GetTokenAsync()
    {
        try
        {
            var result = await _storage.GetAsync<string>(TokenKey);
            if (result.Success)
            {
                CachedToken = result.Value;
                return CachedToken;
            }

            CachedToken = null;
            return null;
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
            await _storage.SetAsync(TokenKey, token);
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
            await _storage.DeleteAsync(TokenKey);
        }
        catch (InvalidOperationException)
        {
        }
    }
}
