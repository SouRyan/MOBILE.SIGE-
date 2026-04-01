using System.Net.Http.Json;
using API.SIGE.DTOs;

namespace API.SIGE.ApiServices;

public class ProducaoFamiliaApiService
{
    private readonly HttpClient _http;

    public ProducaoFamiliaApiService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("ApiSige");
    }

    public async Task<ProducaoFamiliaResponseDto?> GetByFamiliaAsync(int familiaId)
        => await _http.GetFromJsonAsync<ProducaoFamiliaResponseDto>($"api/producao-familia/familia/{familiaId}");

    public async Task<ProducaoFamiliaResponseDto?> GetByIdAsync(int id)
        => await _http.GetFromJsonAsync<ProducaoFamiliaResponseDto>($"api/producao-familia/{id}");

    public async Task<ProducaoFamiliaResponseDto?> IniciarAsync(int familiaId, ProducaoFamiliaIniciarDto dto)
    {
        var response = await _http.PostAsJsonAsync($"api/producao-familia/{familiaId}/iniciar", dto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ProducaoFamiliaResponseDto>();
    }

    public async Task<ProducaoFamiliaResponseDto?> PausarAsync(int familiaId, ProducaoFamiliaPausarDto? dto = null)
    {
        var response = await _http.PostAsJsonAsync($"api/producao-familia/{familiaId}/pausar", dto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ProducaoFamiliaResponseDto>();
    }

    public async Task<ProducaoFamiliaResponseDto?> FinalizarAsync(int familiaId, ProducaoFamiliaFinalizarDto? dto = null)
    {
        var response = await _http.PostAsJsonAsync($"api/producao-familia/{familiaId}/finalizar", dto ?? new ProducaoFamiliaFinalizarDto());
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ProducaoFamiliaResponseDto>();
    }
}
