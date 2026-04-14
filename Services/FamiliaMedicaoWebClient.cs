using System.Net.Http.Json;
using System.Text.Json;

namespace MOBILE.SIGE.Services;

/// <summary>
/// Envia a foto da medição para o site GerenciamentoProducao (armazenamento local + aprovação na web).
/// </summary>
public class FamiliaMedicaoWebClient
{
    private readonly HttpClient _http;
    private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

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

    public async Task<MedicaoFotoResult?> ObterFotoAsync(int idFamilia, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _http.GetAsync($"api/familia-caixilho/{idFamilia}/medicao-foto", cancellationToken);
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<MedicaoFotoResult>(JsonOpts, cancellationToken);
        }
        catch
        {
            return null;
        }
    }

    public class MedicaoFotoResult
    {
        public string FotoBase64 { get; set; } = string.Empty;
        public DateTime EnviadoEm { get; set; }
        public string? EnviadoPor { get; set; }
        public bool Aprovada { get; set; }
    }
}
