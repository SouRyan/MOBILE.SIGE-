using API.SIGE.ApiServices;
using Microsoft.AspNetCore.Components.Authorization;
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

// HttpClient com JWT handler
builder.Services.AddScoped<JwtAuthHandler>();

builder.Services.AddHttpClient("ApiSige", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSige:BaseUrl"] ?? "http://localhost:5075/");
})
.AddHttpMessageHandler<JwtAuthHandler>();

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
//builder.Services.AddScoped<AnexoApiService>();
builder.Services.AddScoped<NotificacaoApiService>();
builder.Services.AddScoped<DashboardApiService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
