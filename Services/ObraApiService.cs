using System.Net.Http.Json;
using MOBILE.SIGE.Models.Obra;

namespace API.SIGE.ApiServices;

public class ObraApiService
{
    private readonly HttpClient _http;

    public ObraApiService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("ApiSige");
    }

    public async Task<List<ObraResponseDto>?> GetAllAsync(bool? finalizadas = null)
    {
        var url = finalizadas.HasValue
            ? $"api/obra?finalizadas={finalizadas.Value}"
            : "api/obra";
        return await _http.GetFromJsonAsync<List<ObraResponseDto>>(url);
    }

    public async Task<ObraResponseDto?> GetByIdAsync(int id)
        => await _http.GetFromJsonAsync<ObraResponseDto>($"api/obra/{id}");

    public async Task<ObraResponseDto?> CreateAsync(ObraCreateDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/obra", dto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ObraResponseDto>();
    }

    public async Task<HttpResponseMessage> UpdateAsync(int id, ObraUpdateDto dto)
        => await _http.PutAsJsonAsync($"api/obra/{id}", dto);

    public async Task<HttpResponseMessage> DeleteAsync(int id)
        => await _http.DeleteAsync($"api/obra/{id}");

    public async Task<HttpResponseMessage> VerificarAsync(int id)
        => await _http.PostAsync($"api/obra/{id}/verificar", null);

    public async Task<HttpResponseMessage> ConcluirAsync(int id)
        => await _http.PostAsync($"api/obra/{id}/concluir", null);
}
