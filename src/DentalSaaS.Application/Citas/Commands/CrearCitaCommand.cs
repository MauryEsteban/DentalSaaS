using MediatR;

namespace DentalSaaS.Application.Citas.Commands;

public record CrearCitaCommand(
    Guid PacienteId, 
    Guid OdontologoId, 
    DateTime FechaInicio, 
    DateTime FechaFin) : IRequest<Guid>;
