using MediatR;

namespace DentalSaaS.Application.BoundedContexts.Clinical.Commands;

// Usamos record para el DTO. Pasamos el JSON crudo que viene desde la UI.
public record SaveEvolucionDraftCommand(Guid FichaClinicaId, Guid OdontologoId, string PayloadJson) : IRequest;
