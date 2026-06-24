using Microsoft.EntityFrameworkCore;
using DentalSaaS.Domain.Entities;
using DentalSaaS.Domain.BoundedContexts.Administration.Entities;
using DentalSaaS.Domain.BoundedContexts.Clinical.Entities;
using DentalSaaS.Application.Common.Interfaces;

namespace DentalSaaS.Infrastructure.Persistence;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options, 
    ITenantService tenantService) : DbContext(options), IApplicationDbContext
{
    public DbSet<Cita> Citas => Set<Cita>();
    public DbSet<Cobro> Cobros => Set<Cobro>();
    public DbSet<Odontologo> Odontologos => Set<Odontologo>();
    public DbSet<Paciente> Pacientes => Set<Paciente>();
    public DbSet<FichaClinica> FichasClinicas => Set<FichaClinica>();
    public DbSet<EvolucionDraft> EvolucionDrafts => Set<EvolucionDraft>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var tenantId = tenantService.TenantId;

        // Global Query Filters
        modelBuilder.Entity<Cita>().HasQueryFilter(x => x.TenantId == tenantId);
        modelBuilder.Entity<Cobro>().HasQueryFilter(x => x.TenantId == tenantId);
        modelBuilder.Entity<Odontologo>().HasQueryFilter(x => x.TenantId == tenantId);
        modelBuilder.Entity<Paciente>().HasQueryFilter(x => x.TenantId == tenantId);
        modelBuilder.Entity<FichaClinica>().HasQueryFilter(x => x.TenantId == tenantId);
        modelBuilder.Entity<Evolucion>().HasQueryFilter(x => x.TenantId == tenantId);
        modelBuilder.Entity<HallazgoOdontologico>().HasQueryFilter(x => x.TenantId == tenantId);
        modelBuilder.Entity<EvolucionDraft>().HasQueryFilter(x => x.TenantId == tenantId);

        // Configuraciones Clínicas
        modelBuilder.Entity<FichaClinica>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            entity.HasOne<Paciente>().WithOne().HasForeignKey<FichaClinica>(f => f.PacienteId).OnDelete(DeleteBehavior.Restrict);
            entity.HasMany(e => e.Evoluciones).WithOne().HasForeignKey(e => e.FichaClinicaId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Evolucion>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            entity.Property(e => e.NotaClinica).IsRequired();
            entity.HasMany(e => e.Hallazgos).WithOne().HasForeignKey(h => h.EvolucionId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<HallazgoOdontologico>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            entity.Property(e => e.NomenclaturaPieza).IsRequired().HasMaxLength(5);
            entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(250);
        });

        // Configuración del Draft (Alta Eficiencia)
        modelBuilder.Entity<EvolucionDraft>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            
            // En SQL Server, usar NVARCHAR(MAX) nos permite guardar el JSON gigante sin límites
            entity.Property(e => e.PayloadJson).IsRequired().HasColumnType("NVARCHAR(MAX)");
            
            // Un índice único para que un Odontólogo solo tenga UN borrador activo por Ficha Clínica
            entity.HasIndex(e => new { e.TenantId, e.FichaClinicaId, e.OdontologoId }).IsUnique();
        });
    }
}
