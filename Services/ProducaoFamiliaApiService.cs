using MOBILE.SIGE.Models.ProducaoFamilia;
using System.Net.Http.Json;
using System.Text.Json;


namespace API.SIGE.ApiServices;

public class ProducaoFamiliaApiService
{
    private readonly HttpClient _http;
    private static readonly JsonSerializerOptions _jsonOpts = new() { PropertyNameCaseInsensitive = true };

    public ProducaoFamiliaApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<ProducaoFamiliaResponseDto?> GetByFamiliaAsync(int familiaId)
        => await _http.GetFromJsonAsync<ProducaoFamiliaResponseDto>($"api/producao-familia/familia/{familiaId}");

    public async Task<ProducaoFamiliaResponseDto?> GetByIdAsync(int id)
        => await _http.GetFromJsonAsync<ProducaoFamiliaResponseDto>($"api/producao-familia/{id}");

    public async Task<ProducaoFamiliaResponseDto?> IniciarAsync(int familiaId, ProducaoFamiliaIniciarDto dto)
    {
        var response = await _http.PostAsJsonAsync($"api/producao-familia/{familiaId}/iniciar", dto);
        await EnsureSuccessOrThrowFriendly(response);
        return await response.Content.ReadFromJsonAsync<ProducaoFamiliaResponseDto>();
    }

    public async Task<ProducaoFamiliaResponseDto?> PausarAsync(int familiaId, ProducaoFamiliaPausarDto? dto = null)
    {
        var response = await _http.PostAsJsonAsync($"api/producao-familia/{familiaId}/pausar", dto);
        await EnsureSuccessOrThrowFriendly(response);
        return await response.Content.ReadFromJsonAsync<ProducaoFamiliaResponseDto>();
    }

    public async Task<ProducaoFamiliaResponseDto?> FinalizarAsync(int familiaId, ProducaoFamiliaFinalizarDto? dto = null)
    {
        var response = await _http.PostAsJsonAsync($"api/producao-familia/{familiaId}/finalizar", dto ?? new ProducaoFamiliaFinalizarDto());
        await EnsureSuccessOrThrowFriendly(response);
        return await response.Content.ReadFromJsonAsync<ProducaoFamiliaResponseDto>();
    }

    private static async Task EnsureSuccessOrThrowFriendly(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode) return;

        // Tentar ler mensagem de erro da API
        string? mensagem = null;
        try
        {
            var body = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrWhiteSpace(body))
            {
                var json = JsonSerializer.Deserialize<JsonElement>(body, _jsonOpts);
                if (json.TryGetProperty("message", out var msg))
                    mensagem = msg.GetString();
            }
        }
        catch { }

        throw new HttpRequestException(
            mensagem ?? $"Erro ao processar a solicitacao ({(int)response.StatusCode}).",
            null,
            response.StatusCode);
    }
}
