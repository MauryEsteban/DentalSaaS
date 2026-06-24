using Microsoft.EntityFrameworkCore;
using DentalSaaS.Domain.Entities;
using DentalSaaS.Domain.BoundedContexts.Administration.Entities;
using DentalSaaS.Domain.BoundedContexts.Clinical.Entities;

namespace DentalSaaS.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Cita> Citas { get; }
    DbSet<Cobro> Cobros { get; }
    
    // Bounded Context: Administration
    DbSet<Odontologo> Odontologos { get; }
    
    // Bounded Context: Clinical
    DbSet<Paciente> Pacientes { get; }
    DbSet<FichaClinica> FichasClinicas { get; }
    DbSet<EvolucionDraft> EvolucionDrafts { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
