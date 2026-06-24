using System;
using System.ComponentModel.DataAnnotations;

namespace DentalSaaS.Blazor.Models;

public class CitaCreateDto
{
    [Required(ErrorMessage = "El ID del paciente es obligatorio")]
    public Guid PacienteId { get; set; }

    [Required(ErrorMessage = "El ID del odontólogo es obligatorio")]
    public Guid OdontologoId { get; set; }

    [Required]
    public DateTime FechaInicio { get; set; } = DateTime.Now;

    [Required]
    public DateTime FechaFin { get; set; } = DateTime.Now.AddHours(1);
}
