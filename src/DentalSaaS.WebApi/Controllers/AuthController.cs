using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DentalSaaS.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(IConfiguration config) : ControllerBase
{
    // Mock de base de datos global de acceso
    private static readonly Dictionary<string, List<TenantAccessDto>> UserTenantsMock = new()
    {
        { "admin", new List<TenantAccessDto> { 
            new("Clinica-Centro-001", "Clínica Dental Centro"), 
            new("Clinica-Sur-002", "Sucursal Sur") 
        }}
    };

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // 1. Validar credenciales (Fase 1)
        if (request.Usuario == "admin" && request.Password == "1234")
        {
            var clinics = UserTenantsMock[request.Usuario];
            
            // Si solo tiene 1 clínica, podríamos saltarnos el paso 2 en el front.
            // Retornamos las clínicas disponibles para que el UI las muestre.
            return Ok(new { 
                Message = "Credenciales válidas. Seleccione una clínica.", 
                RequireTenantSelection = true, 
                AvailableTenants = clinics 
            });
        }

        return Unauthorized(new { Message = "Usuario o contraseña incorrectos" });
    }

    [HttpPost("select-tenant")]
    public IActionResult SelectTenant([FromBody] SelectTenantRequest request)
    {
        // 2. Emitir JWT Scoped (Fase 2)
        // En la vida real, aquí re-validamos que "admin" realmente tiene acceso a "request.TenantId"
        if (request.Usuario == "admin" && UserTenantsMock["admin"].Any(t => t.TenantId == request.TenantId))
        {
            var jwtSettings = config.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, request.Usuario),
                new Claim("TenantId", request.TenantId), // <-- EL SECRETO DE LA ARQUITECTURA AISLADA
                new Claim(ClaimTypes.Role, "Odontologo")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return Ok(new { Token = tokenHandler.WriteToken(token), TenantId = request.TenantId });
        }

        return Forbid();
    }
}

public record LoginRequest(string Usuario, string Password);
public record SelectTenantRequest(string Usuario, string TenantId);
public record TenantAccessDto(string TenantId, string NombreClinica);
