using MOBILE.SIGE.Models.Cargo;
using System.Net.Http.Json;


namespace API.SIGE.ApiServices;

public class CargoApiService
{
    private readonly HttpClient _http;

    public CargoApiService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("ApiSige");
    }

    public async Task<List<CargoResponseDto>?> GetAllAsync()
        => await _http.GetFromJsonAsync<List<CargoResponseDto>>("api/cargo");

    public async Task<CargoResponseDto?> GetByIdAsync(int id)
        => await _http.GetFromJsonAsync<CargoResponseDto>($"api/cargo/{id}");

    public async Task<CargoResponseDto?> CreateAsync(CargoCreateDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/cargo", dto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CargoResponseDto>();
    }

    public async Task<HttpResponseMessage> UpdateAsync(int id, CargoCreateDto dto)
        => await _http.PutAsJsonAsync($"api/cargo/{id}", dto);

    public async Task<HttpResponseMessage> DeleteAsync(int id)
        => await _http.DeleteAsync($"api/cargo/{id}");
}
