using DentalSaaS.Domain.Common;

namespace DentalSaaS.Domain.BoundedContexts.Administration.Entities;

public class Odontologo : BaseEntity
{
    public string NombreCompleto { get; private set; }
    public string NumeroColegiado { get; private set; }
    public string Especialidad { get; private set; }
    public bool Activo { get; private set; }

    // Referencia al Identity User (Auth0, Azure AD, o ASP.NET Identity)
    public string? IdentityUserId { get; private set; } 

    private Odontologo() { } // EF Core

    public Odontologo(string nombreCompleto, string numeroColegiado, string especialidad, string? identityUserId = null)
    {
        if (string.IsNullOrWhiteSpace(nombreCompleto)) throw new ArgumentException("El nombre es requerido.");
        
        NombreCompleto = nombreCompleto;
        NumeroColegiado = numeroColegiado;
        Especialidad = especialidad;
        IdentityUserId = identityUserId;
        Activo = true;
    }

    public void Desactivar() => Activo = false;
    public void Activar() => Activo = true;
    
    public void ActualizarPerfil(string nombreCompleto, string especialidad)
    {
        NombreCompleto = nombreCompleto;
        Especialidad = especialidad;
    }
}
