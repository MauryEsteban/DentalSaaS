using DentalSaaS.Domain.Common;

namespace DentalSaaS.Domain.Entities;

public class Cobro : BaseEntity
{
    public Guid CitaId { get; private set; }
    public decimal Monto { get; private set; }

    private Cobro() { } // Para EF Core

    public Cobro(Guid citaId, decimal monto)
    {
        CitaId = citaId;
        Monto = monto;
    }
}
