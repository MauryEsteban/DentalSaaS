namespace DentalSaaS.Application.Common.Models;

public record CitaCalendarioDto(
    Guid Id,
    Guid PacienteId,
    string NombrePaciente,
    Guid OdontologoId,
    DateTime FechaInicio,
    DateTime FechaFin,
    string Estado
);