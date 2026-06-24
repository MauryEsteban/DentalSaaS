using System.Security.Claims;
using DentalSaaS.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace DentalSaaS.WebApi.Services;

public class TenantService(IHttpContextAccessor httpContextAccessor) : ITenantService
{
    public string TenantId 
    {
        get 
        {
            var context = httpContextAccessor.HttpContext;
            if (context is null) return "Tenant-Default";

            // 1. Prioridad Máxima: Leer desde el Token JWT (Arquitectura B2B Real)
            var tenantClaim = context.User.Claims.FirstOrDefault(c => c.Type == "TenantId");
            if (tenantClaim is not null && !string.IsNullOrWhiteSpace(tenantClaim.Value))
            {
                return tenantClaim.Value;
            }
            
            // 2. Fallback: Leer desde el Header (Útil para pruebas en Swagger/Postman)
            return context.Request.Headers["X-Tenant-Id"].FirstOrDefault() ?? "Tenant-Default";
        }
    }
}
