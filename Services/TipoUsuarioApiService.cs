using System.Net.Http.Json;
using API.SIGE.DTOs;

namespace API.SIGE.ApiServices;

public class TipoUsuarioApiService
{
    private readonly HttpClient _http;

    public TipoUsuarioApiService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("ApiSige");
    }

    public async Task<List<TipoUsuarioResponseDto>?> GetAllAsync()
        => await _http.GetFromJsonAsync<List<TipoUsuarioResponseDto>>("api/tipo-usuario");

    public async Task<TipoUsuarioResponseDto?> GetByIdAsync(int id)
        => await _http.GetFromJsonAsync<TipoUsuarioResponseDto>($"api/tipo-usuario/{id}");

    public async Task<TipoUsuarioResponseDto?> CreateAsync(TipoUsuarioCreateDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/tipo-usuario", dto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TipoUsuarioResponseDto>();
    }

    public async Task<HttpResponseMessage> UpdateAsync(int id, TipoUsuarioCreateDto dto)
        => await _http.PutAsJsonAsync($"api/tipo-usuario/{id}", dto);

    public async Task<HttpResponseMessage> DeleteAsync(int id)
        => await _http.DeleteAsync($"api/tipo-usuario/{id}");
}
