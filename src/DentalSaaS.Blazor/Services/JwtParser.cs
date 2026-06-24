using System.Security.Claims;
using System.Text.Json;

namespace DentalSaaS.Blazor.Services;

public static class JwtParser
{
    public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();

        // VALIDACIÓN: Si el token es nulo, vacío o no tiene los 3 puntos de un JWT, abortamos
        if (string.IsNullOrWhiteSpace(jwt) || !jwt.Contains("."))
        {
            return claims;
        }

        var parts = jwt.Split('.');
        if (parts.Length < 2)
        {
            return claims;
        }

        var payload = parts[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);

        try
        {
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            if (keyValuePairs != null)
            {
                claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString() ?? "")));
            }
        }
        catch (Exception)
        {
            // Si el JSON está mal formado, devolvemos lista vacía en lugar de romper la app
        }

        return claims;
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
}