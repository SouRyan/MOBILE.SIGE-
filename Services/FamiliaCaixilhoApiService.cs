using MOBILE.SIGE.Models.FamiliaCaixilho;
using System.Net.Http.Json;


namespace API.SIGE.ApiServices;

public class FamiliaCaixilhoApiService
{
    private readonly HttpClient _http;

    public FamiliaCaixilhoApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<FamiliaCaixilhoResponseDto>?> GetAllAsync()
        => await _http.GetFromJsonAsync<List<FamiliaCaixilhoResponseDto>>("api/familia-caixilho");

    public async Task<List<FamiliaCaixilhoResponseDto>?> GetByObraAsync(int obraId)
        => await _http.GetFromJsonAsync<List<FamiliaCaixilhoResponseDto>>($"api/familia-caixilho/obra/{obraId}");

    public async Task<FamiliaCaixilhoResponseDto?> GetByIdAsync(int id)
        => await _http.GetFromJsonAsync<FamiliaCaixilhoResponseDto>($"api/familia-caixilho/{id}");

    public async Task<FamiliaCaixilhoResponseDto?> CreateAsync(FamiliaCaixilhoCreateDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/familia-caixilho", dto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<FamiliaCaixilhoResponseDto>();
    }

    public async Task<HttpResponseMessage> UpdateAsync(int id, FamiliaCaixilhoUpdateDto dto)
        => await _http.PutAsJsonAsync($"api/familia-caixilho/{id}", dto);

    public async Task<HttpResponseMessage> DeleteAsync(int id)
        => await _http.DeleteAsync($"api/familia-caixilho/{id}");

    public async Task<HttpResponseMessage> RecalcularPesosAsync()
        => await _http.PostAsync("api/familia-caixilho/recalcular-pesos", null);
}
