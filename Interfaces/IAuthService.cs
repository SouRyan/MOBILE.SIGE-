using MOBILE.SIGE.Models.Login;

namespace MOBILE.SIGE.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task LogoutAsync();
        Task<bool> IsAuthenticatedAsync();
    }
}
