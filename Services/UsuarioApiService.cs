using MOBILE.SIGE.Models.Enums;
using MOBILE.SIGE.Models.Usuario;
using System.Net.Http.Json;


namespace API.SIGE.ApiServices;

public class UsuarioApiService
{
    private readonly HttpClient _http;

    public UsuarioApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<UsuarioResponseDto>?> GetAllAsync(bool ativos = true)
        => await _http.GetFromJsonAsync<List<UsuarioResponseDto>>($"api/usuario?ativos={ativos}");

    public async Task<List<UsuarioResponseDto>?> GetInativosAsync()
        => await _http.GetFromJsonAsync<List<UsuarioResponseDto>>("api/usuario/inativos");

    public async Task<UsuarioResponseDto?> GetByIdAsync(int id)
        => await _http.GetFromJsonAsync<UsuarioResponseDto>($"api/usuario/{id}");

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
    {
        var response = await _http.PostAsJsonAsync("api/usuario/login", request);
        return await response.Content.ReadFromJsonAsync<LoginResponseDto>();
    }

    public async Task<UsuarioResponseDto?> CreateAsync(UsuarioCreateDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/usuario", dto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<UsuarioResponseDto>();
    }

    public async Task<HttpResponseMessage> UpdateAsync(int id, UsuarioUpdateDto dto)
        => await _http.PutAsJsonAsync($"api/usuario/{id}", dto);

    public async Task<HttpResponseMessage> InativarAsync(int id)
        => await _http.DeleteAsync($"api/usuario/{id}");

    public async Task<HttpResponseMessage> AtivarAsync(int id)
        => await _http.PostAsync($"api/usuario/{id}/ativar", null);

    public async Task<HttpResponseMessage> AtribuirCargoAsync(int id, int idCargo)
        => await _http.PostAsync($"api/usuario/{id}/cargo/{idCargo}", null);

    public async Task<HttpResponseMessage> RemoverCargoAsync(int id)
        => await _http.DeleteAsync($"api/usuario/{id}/cargo");

    public async Task<List<UsuarioResponseDto>?> GetByCargoAsync(TipoCargo tipoCargo)
        => await _http.GetFromJsonAsync<List<UsuarioResponseDto>>($"api/usuario/cargo/{tipoCargo}");
}
