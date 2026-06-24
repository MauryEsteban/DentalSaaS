using MediatR;
using DentalSaaS.Application.Common.Interfaces;

namespace DentalSaaS.Application.Citas.Commands;

public class CancelarCitaHandler(IApplicationDbContext context) : IRequestHandler<CancelarCitaCommand>
{
    public async Task Handle(CancelarCitaCommand request, CancellationToken cancellationToken)
    {
        var cita = await context.Citas.FindAsync([request.CitaId], cancellationToken)
            ?? throw new KeyNotFoundException("La cita que intenta cancelar no existe.");

        // El Aggregate Root controla sus propias invariantes. Cero lógica de negocio aquí.
        cita.Cancelar();

        await context.SaveChangesAsync(cancellationToken);
    }
}
