using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace DentalSaaS.Blazor.Services;

public class AuthService(HttpClient httpClient, ProtectedLocalStorage localStorage)
{
    // FASE 1: Devuelve la lista de clínicas si las credenciales son correctas
    public async Task<Phase1Response?> LoginFase1(string usuario, string password)
    {
        var response = await httpClient.PostAsJsonAsync("api/v1/Auth/login", new { Usuario = usuario, Password = password });
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Phase1Response>();
        }
        return null;
    }

    // FASE 2: Selecciona el Tenant y guarda el Token definitivo
    public async Task<string?> LoginFase2(string usuario, string tenantId)
    {
        var response = await httpClient.PostAsJsonAsync("api/v1/Auth/select-tenant", 
            new { Usuario = usuario, TenantId = tenantId });

        if (response.IsSuccessStatusCode)
        {
            var tokenResult = await response.Content.ReadFromJsonAsync<Phase2Response>();
            if (tokenResult != null && !string.IsNullOrWhiteSpace(tokenResult.Token))
            {
                await localStorage.SetAsync("authToken", tokenResult.Token);
                return tokenResult.Token;
            }
        }
        return null;
    }

    public async Task<string?> GetToken()
    {
        var result = await localStorage.GetAsync<string>("authToken");
        return result.Success ? result.Value : null;
    }

    public async Task Logout() => await localStorage.DeleteAsync("authToken");
}

public class Phase1Response 
{ 
    public string Message { get; set; } = string.Empty;
    public bool RequireTenantSelection { get; set; } 
    public List<TenantDto> AvailableTenants { get; set; } = []; 
}
public class TenantDto { public string TenantId { get; set; } = string.Empty; public string NombreClinica { get; set; } = string.Empty; }
public class Phase2Response { public string Token { get; set; } = string.Empty; }
