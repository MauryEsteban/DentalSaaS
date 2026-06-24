using MediatR;
using Microsoft.EntityFrameworkCore;
using DentalSaaS.Application.Common.Interfaces;

namespace DentalSaaS.Application.Citas.Commands;

public class ReprogramarCitaHandler(IApplicationDbContext context) : IRequestHandler<ReprogramarCitaCommand>
{
    public async Task Handle(ReprogramarCitaCommand request, CancellationToken cancellationToken)
    {
        var cita = await context.Citas.FindAsync([request.CitaId], cancellationToken)
            ?? throw new KeyNotFoundException("La cita solicitada no existe.");

        // Verificamos colisión ignorando la cita actual que estamos moviendo
        bool existeSolapamiento = await context.Citas
            .AnyAsync(c => c.OdontologoId == cita.OdontologoId &&
                           c.Id != cita.Id && // Omitir la cita actual en la comprobación
                           c.Estado != Domain.Enums.EstadoCita.Cancelada &&
                           c.FechaInicio < request.FechaFin &&
                           c.FechaFin > request.FechaInicio,
                      cancellationToken);

        if (existeSolapamiento)
            throw new InvalidOperationException("El nuevo bloque horario colisiona con otra cita del odontólogo.");

        cita.Reprogramar(request.FechaInicio, request.FechaFin);
        await context.SaveChangesAsync(cancellationToken);
    }
}