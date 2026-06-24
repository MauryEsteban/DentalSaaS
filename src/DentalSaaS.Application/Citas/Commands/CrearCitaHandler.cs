using MediatR;
using Microsoft.EntityFrameworkCore;
using DentalSaaS.Application.Common.Interfaces;
using DentalSaaS.Domain.Entities;

namespace DentalSaaS.Application.Citas.Commands;

public class CrearCitaHandler(IApplicationDbContext context) : IRequestHandler<CrearCitaCommand, Guid>
{
    public async Task<Guid> Handle(CrearCitaCommand request, CancellationToken cancellationToken)
    {
        bool existeSolapamiento = await context.Citas
            .AnyAsync(c => c.OdontologoId == request.OdontologoId &&
                           c.Estado != Domain.Enums.EstadoCita.Cancelada &&
                           c.FechaInicio < request.FechaFin &&
                           c.FechaFin > request.FechaInicio, 
                      cancellationToken);

        if (existeSolapamiento)
        {
            throw new InvalidOperationException("""
                No se puede agendar la cita. El odontólogo ya tiene 
                una cita programada que se solapa con este bloque horario.
                """);
        }

        var nuevaCita = new Cita(
            request.PacienteId,
            request.OdontologoId,
            request.FechaInicio,
            request.FechaFin
        );

        context.Citas.Add(nuevaCita);
        await context.SaveChangesAsync(cancellationToken);

        return nuevaCita.Id;
    }
}
