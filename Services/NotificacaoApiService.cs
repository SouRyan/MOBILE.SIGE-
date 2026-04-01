using System.Net.Http.Json;
using API.SIGE.DTOs;

namespace API.SIGE.ApiServices;

public class NotificacaoApiService
{
    private readonly HttpClient _http;

    public NotificacaoApiService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("ApiSige");
    }

    public async Task<List<NotificacaoResponseDto>?> GetByUsuarioAsync(int idUsuario)
        => await _http.GetFromJsonAsync<List<NotificacaoResponseDto>>($"api/notificacao/{idUsuario}");

    public async Task<HttpResponseMessage> MarcarLidaAsync(int id)
        => await _http.PostAsync($"api/notificacao/{id}/marcar-lida", null);
}
