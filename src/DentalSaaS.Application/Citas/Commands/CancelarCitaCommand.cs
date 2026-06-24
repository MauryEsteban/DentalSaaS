using MediatR;
namespace DentalSaaS.Application.Citas.Commands;

public record CancelarCitaCommand(Guid CitaId) : IRequest;