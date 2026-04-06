using System.Net.Http.Json;

using MOBILE.SIGE.Models.Dashboard;

namespace API.SIGE.ApiServices;

public class HomeApiService
{
    private readonly HttpClient _http;

    public HomeApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<DashboardMetricasDto?> GetDashboardAsync()
        => await _http.GetFromJsonAsync<DashboardMetricasDto>("api/home/dashboard");

    public async Task<HttpResponseMessage> ImportarObrasXmlAsync(Stream fileStream, string fileName)
    {
        using var content = new MultipartFormDataContent();
        content.Add(new StreamContent(fileStream), "arquivosXml", fileName);

        var response = await _http.PostAsync("api/home/importar-obras-xml", content);
        response.EnsureSuccessStatusCode();
        return response;
    }

    public async Task<HttpResponseMessage> ImportarObrasXmlAsync(List<(Stream Stream, string FileName)> arquivos)
    {
        using var content = new MultipartFormDataContent();
        foreach (var (stream, name) in arquivos)
        {
            content.Add(new StreamContent(stream), "arquivosXml", name);
        }

        var response = await _http.PostAsync("api/home/importar-obras-xml", content);
        response.EnsureSuccessStatusCode();
        return response;
    }
}
