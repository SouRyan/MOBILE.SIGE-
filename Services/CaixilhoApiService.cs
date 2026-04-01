using System.Net.Http.Json;
using ;

namespace API.SIGE.ApiServices;

public class CaixilhoApiService
{
    private readonly HttpClient _http;

    public CaixilhoApiService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("ApiSige");
    }

    public async Task<List<CaixilhoResponseDto>?> GetAllAsync()
        => await _http.GetFromJsonAsync<List<CaixilhoResponseDto>>("api/caixilho");

    public async Task<CaixilhoResponseDto?> GetByIdAsync(int id)
        => await _http.GetFromJsonAsync<CaixilhoResponseDto>($"api/caixilho/{id}");

    public async Task<CaixilhoResponseDto?> CreateAsync(CaixilhoCreateDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/caixilho", dto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CaixilhoResponseDto>();
    }

    public async Task<HttpResponseMessage> UpdateAsync(int id, CaixilhoUpdateDto dto)
        => await _http.PutAsJsonAsync($"api/caixilho/{id}", dto);

    public async Task<HttpResponseMessage> DeleteAsync(int id)
        => await _http.DeleteAsync($"api/caixilho/{id}");

    public async Task<HttpResponseMessage> LiberarAsync(int id)
        => await _http.PostAsync($"api/caixilho/{id}/liberar", null);
}
