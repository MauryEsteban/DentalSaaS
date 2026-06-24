using MediatR;

namespace DentalSaaS.Application.BoundedContexts.Clinical.Commands;

// Retorna el Guid de la Ficha Clínica recién creada
public record CrearPacientePruebaCommand() : IRequest<Guid>;
