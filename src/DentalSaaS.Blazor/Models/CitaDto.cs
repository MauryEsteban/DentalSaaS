using System;

namespace DentalSaaS.Blazor.Models;

public class CitaDto
{
    public Guid Id { get; set; }
    public Guid PacienteId { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
}
