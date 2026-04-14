using API.SIGE.ApiServices;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MOBILE.SIGE.ApiServices;
using MOBILE.SIGE.Components;
using MOBILE.SIGE.Interfaces;
using MOBILE.SIGE.Services;
using MOBILE.SIGE.Services.Auth;

var builder = WebApplication.CreateBuilder(args);

static Uri GetSafeBaseUri(IConfiguration configuration, string key, string fallbackUrl)
{
    var raw = configuration[key]?.Trim();
    var candidate = string.IsNullOrWhiteSpace(raw) ? fallbackUrl : raw;

    if (!candidate.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
        !candidate.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
    {
        candidate = $"https://{candidate}";
    }

    if (!candidate.EndsWith('/'))
    {
        candidate += "/";
    }

    if (Uri.TryCreate(candidate, UriKind.Absolute, out var uri))
    {
        return uri;
    }

    return new Uri(fallbackUrl.EndsWith('/') ? fallbackUrl : $"{fallbackUrl}/");
}

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Auth
builder.Services.AddScoped<TokenStorageService>();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<CustomAuthStateProvider>());
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<ProtectedLocalStorage>();

// HttpClient scoped ao circuito Blazor, com JWT handler
builder.Services.AddScoped(sp =>
{
    var tokenStorage = sp.GetRequiredService<TokenStorageService>();
    var handler = new JwtAuthHandler(tokenStorage)
    {
        InnerHandler = new HttpClientHandler()
    };
    var baseUri = GetSafeBaseUri(builder.Configuration, "ApiSige:BaseUrl", "http://localhost:5075/");
    return new HttpClient(handler) { BaseAddress = baseUri };
});

// Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<UsuarioApiService>();
builder.Services.AddScoped<ObraApiService>();
builder.Services.AddScoped<CaixilhoApiService>();
builder.Services.AddScoped<FamiliaCaixilhoApiService>();
builder.Services.AddScoped<CargoApiService>();
builder.Services.AddScoped<TipoUsuarioApiService>();
builder.Services.AddScoped<MedicaoApiService>();
builder.Services.AddScoped<ProducaoFamiliaApiService>();
builder.Services.AddScoped<AnexoApiService>();
builder.Services.AddScoped<NotificacaoApiService>();
builder.Services.AddScoped<DashboardApiService>();

builder.Services.AddScoped(sp =>
{
    var cfg = sp.GetRequiredService<IConfiguration>();
    var baseUri = GetSafeBaseUri(cfg, "GerenciamentoWeb:BaseUrl", "http://localhost:5192/");

    var tokenStorage = sp.GetRequiredService<TokenStorageService>();
    var handler = new JwtAuthHandler(tokenStorage)
    {
        InnerHandler = new HttpClientHandler()
    };
    var http = new HttpClient(handler) { BaseAddress = baseUri };
    return new FamiliaMedicaoWebClient(http);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseAntiforgery();
app.UseStaticFiles();

var manifestPath = Path.Combine(app.Environment.ContentRootPath, "Pwa", "manifest.webmanifest");
app.MapGet("/manifest.webmanifest", () =>
    File.Exists(manifestPath)
        ? Results.File(manifestPath, "application/manifest+json")
        : Results.NotFound());

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
