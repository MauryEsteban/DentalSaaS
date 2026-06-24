using DentalSaaS.Blazor.Data;
using DentalSaaS.Blazor.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURACIÓN DE SERVICIOS (Lo que antes era ConfigureServices) ---
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor().AddHubOptions(options =>
{
    options.MaximumReceiveMessageSize = 1024 * 1024; // 100 KB para permitir tokens largos
});

// Registro del HttpClient para conectar con la API
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5044/")
});

// Almacenamiento protegido y servicios personalizados
builder.Services.AddDataProtection();
builder.Services.AddScoped<AuthService>();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

var app = builder.Build();

// --- 2. CONFIGURACIÓN DEL PIPELINE (Lo que antes era Configure) ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();