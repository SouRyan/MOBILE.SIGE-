using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components;

namespace MOBILE.SIGE.Services.Auth
{
    public class JwtAuthHandler : DelegatingHandler
    {
        private readonly TokenStorageService _tokenStorage;
        private readonly NavigationManager _navigation;

        public JwtAuthHandler(TokenStorageService tokenStorage, NavigationManager navigation)
        {
            _tokenStorage = tokenStorage;
            _navigation = navigation;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _tokenStorage.GetTokenAsync();
            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await _tokenStorage.RemoveTokenAsync();
                _navigation.NavigateTo("/login", true);
            }

            return response;
        }
    }
}
