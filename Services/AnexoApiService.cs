using MOBILE.SIGE.Models.Anexo;
using MOBILE.SIGE.Models.Enums;


namespace API.SIGE.ApiServices;

public class AnexoApiService
{
    private readonly HttpClient _http;

    public AnexoApiService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("ApiSige");
    }

    public async Task<AnexoResponseDto?> UploadAsync(Stream fileStream, string fileName, TipoAnexo tipoAnexo, int idUsuario, int? idMedicao = null, int? idProducaoFamilia = null)
    {
        using var content = new MultipartFormDataContent();
        content.Add(new StreamContent(fileStream), "Arquivo", fileName);
        content.Add(new StringContent(((int)tipoAnexo).ToString()), "TipoAnexo");
        content.Add(new StringContent(idUsuario.ToString()), "IdUsuario");

        if (idMedicao.HasValue)
            content.Add(new StringContent(idMedicao.Value.ToString()), "IdMedicao");
        if (idProducaoFamilia.HasValue)
            content.Add(new StringContent(idProducaoFamilia.Value.ToString()), "IdProducaoFamilia");

        var response = await _http.PostAsync("api/anexo/upload", content);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<AnexoResponseDto>();
    }

    public async Task<List<AnexoResponseDto>?> GetByMedicaoAsync(int medicaoId)
        => await _http.GetFromJsonAsync<List<AnexoResponseDto>>($"api/anexo/medicao/{medicaoId}");

    public async Task<List<AnexoResponseDto>?> GetByProducaoAsync(int producaoId)
        => await _http.GetFromJsonAsync<List<AnexoResponseDto>>($"api/anexo/producao/{producaoId}");

    public async Task<HttpResponseMessage> DeleteAsync(int id)
        => await _http.DeleteAsync($"api/anexo/{id}");
}
