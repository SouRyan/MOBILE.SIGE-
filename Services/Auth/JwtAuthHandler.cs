using System.Net;
using System.Net.Http.Headers;

namespace MOBILE.SIGE.Services.Auth
{
    public class JwtAuthHandler : DelegatingHandler
    {
        private readonly TokenStorageService _tokenStorage;

        public JwtAuthHandler(TokenStorageService tokenStorage)
        {
            _tokenStorage = tokenStorage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = _tokenStorage.CachedToken;

            if (string.IsNullOrWhiteSpace(token))
                token = await _tokenStorage.GetTokenAsync();

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await _tokenStorage.RemoveTokenAsync();
            }

            return response;
        }
    }
}
