using MOBILE.SIGE.Models.Dashboard;
using System.Net.Http.Json;


namespace API.SIGE.ApiServices;

public class DashboardApiService
{
    private readonly HttpClient _http;

    public DashboardApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<DashboardMetricasDto?> GetMetricasAsync()
        => await _http.GetFromJsonAsync<DashboardMetricasDto>("api/dashboard/metricas");

    public async Task<HttpResponseMessage> AtualizarProgressoObrasAsync()
        => await _http.PostAsync("api/dashboard/atualizar-progresso-obras", null);
}
