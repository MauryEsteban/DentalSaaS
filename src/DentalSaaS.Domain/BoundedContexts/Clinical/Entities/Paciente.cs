using DentalSaaS.Domain.Common;

namespace DentalSaaS.Domain.BoundedContexts.Clinical.Entities;

public class Paciente : BaseEntity
{
    public string Rut { get; private set; }
    public string Nombres { get; private set; }
    public string Apellidos { get; private set; }
    public DateTime FechaNacimiento { get; private set; }
    public string Telefono { get; private set; }
    public string Email { get; private set; }

    private Paciente() { } // EF Core

    public Paciente(string rut, string nombres, string apellidos, DateTime fechaNacimiento, string telefono, string email)
    {
        // En un escenario real, aquí iría un ValueObject 'Rut' que valide el dígito verificador.
        if (string.IsNullOrWhiteSpace(nombres)) throw new ArgumentException("Los nombres son requeridos.");
        if (string.IsNullOrWhiteSpace(apellidos)) throw new ArgumentException("Los apellidos son requeridos.");

        Rut = rut ?? string.Empty;
        Nombres = nombres;
        Apellidos = apellidos;
        FechaNacimiento = fechaNacimiento;
        Telefono = telefono ?? string.Empty;
        Email = email ?? string.Empty;
    }

    public void ActualizarContacto(string telefono, string email)
    {
        Telefono = telefono;
        Email = email;
    }
}
