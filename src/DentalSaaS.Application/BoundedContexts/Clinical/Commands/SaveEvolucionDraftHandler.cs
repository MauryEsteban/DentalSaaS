using MediatR;
using Microsoft.EntityFrameworkCore;
using DentalSaaS.Application.Common.Interfaces;
using DentalSaaS.Domain.BoundedContexts.Clinical.Entities;

namespace DentalSaaS.Application.BoundedContexts.Clinical.Commands;

public class SaveEvolucionDraftHandler(IApplicationDbContext context) : IRequestHandler<SaveEvolucionDraftCommand>
{
    public async Task Handle(SaveEvolucionDraftCommand request, CancellationToken cancellationToken)
    {
        // Buscamos si ya existe un borrador para este odontólogo y ficha
        var draft = await context.EvolucionDrafts
            .FirstOrDefaultAsync(d => d.FichaClinicaId == request.FichaClinicaId 
                                   && d.OdontologoId == request.OdontologoId, 
                                 cancellationToken);

        if (draft is null)
        {
            // Insert
            draft = new EvolucionDraft(request.FichaClinicaId, request.OdontologoId, request.PayloadJson);
            context.EvolucionDrafts.Add(draft);
        }
        else
        {
            // Update - El Aggregate Root se encarga de mutar su estado
            draft.ActualizarPayload(request.PayloadJson);
        }

        // Un solo hit a la BD
        await context.SaveChangesAsync(cancellationToken);
    }
}
