using System.Net.Http.Json;

namespace MOBILE.SIGE.Services;

/// <summary>
/// Envia a foto da medição para o site GerenciamentoProducao (armazenamento local + aprovação na web).
/// </summary>
public class FamiliaMedicaoWebClient
{
    private readonly HttpClient _http;

    public FamiliaMedicaoWebClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<(bool ok, string? mensagem)> EnviarFotoAsync(int idFamilia, string fotoBase64, CancellationToken cancellationToken = default)
    {
        using var response = await _http.PostAsJsonAsync(
            $"api/familia-caixilho/{idFamilia}/medicao-foto",
            new { fotoBase64 },
            cancellationToken);

        if (response.IsSuccessStatusCode)
            return (true, null);

        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        return (false, $"{(int)response.StatusCode}: {body}");
    }
}
