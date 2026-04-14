using API.SIGE.ApiServices;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MOBILE.SIGE.ApiServices;
using MOBILE.SIGE.Components;
using MOBILE.SIGE.Interfaces;
using MOBILE.SIGE.Services;
using MOBILE.SIGE.Services.Auth;

var builder = WebApplication.CreateBuilder(args);

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
    var baseUrl = builder.Configuration["ApiSige:BaseUrl"] ?? "http://localhost:5075/";
    return new HttpClient(handler) { BaseAddress = new Uri(baseUrl) };
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
    var baseUrl = cfg["GerenciamentoWeb:BaseUrl"]?.Trim();
    if (string.IsNullOrEmpty(baseUrl))
        baseUrl = "http://localhost:5192/";
    if (!baseUrl.EndsWith('/'))
        baseUrl += "/";

    var tokenStorage = sp.GetRequiredService<TokenStorageService>();
    var handler = new JwtAuthHandler(tokenStorage)
    {
        InnerHandler = new HttpClientHandler()
    };
    var http = new HttpClient(handler) { BaseAddress = new Uri(baseUrl) };
    return new FamiliaMedicaoWebClient(http);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseAntiforgery();

var manifestPath = Path.Combine(app.Environment.ContentRootPath, "Pwa", "manifest.webmanifest");
app.MapGet("/manifest.webmanifest", () =>
    File.Exists(manifestPath)
        ? Results.File(manifestPath, "application/manifest+json")
        : Results.NotFound());

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
