using System.Net.Http.Json;
using MOBILE.SIGE.Models.Medicao;

namespace API.SIGE.ApiServices;

public class MedicaoApiService
{
    private readonly HttpClient _http;

    public MedicaoApiService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("ApiSige");
    }

    public async Task<MedicaoResponseDto?> GetByFamiliaAsync(int familiaId)
        => await _http.GetFromJsonAsync<MedicaoResponseDto>($"api/medicao/familia/{familiaId}");

    public async Task<MedicaoResponseDto?> GetByIdAsync(int id)
        => await _http.GetFromJsonAsync<MedicaoResponseDto>($"api/medicao/{id}");

    public async Task<MedicaoResponseDto?> IniciarAsync(int familiaId, MedicaoIniciarDto dto)
    {
        var response = await _http.PostAsJsonAsync($"api/medicao/{familiaId}/iniciar", dto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<MedicaoResponseDto>();
    }

    public async Task<MedicaoResponseDto?> PausarAsync(int familiaId, MedicaoPausarDto? dto = null)
    {
        var response = await _http.PostAsJsonAsync($"api/medicao/{familiaId}/pausar", dto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<MedicaoResponseDto>();
    }

    public async Task<MedicaoResponseDto?> FinalizarAsync(int familiaId, MedicaoFinalizarDto? dto = null)
    {
        var response = await _http.PostAsJsonAsync($"api/medicao/{familiaId}/finalizar", dto ?? new MedicaoFinalizarDto());
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<MedicaoResponseDto>();
    }
}
