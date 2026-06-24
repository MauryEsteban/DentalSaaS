using MediatR;
namespace DentalSaaS.Application.Citas.Commands;

public record ReprogramarCitaCommand(Guid CitaId, DateTime FechaInicio, DateTime FechaFin) : IRequest;