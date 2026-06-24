using DentalSaaS.Domain.Common;
using DentalSaaS.Domain.BoundedContexts.Clinical.Enums;

namespace DentalSaaS.Domain.BoundedContexts.Clinical.Entities;

public class HallazgoOdontologico : BaseEntity
{
    public Guid EvolucionId { get; private set; }
    public string NomenclaturaPieza { get; private set; } // Ej: "14", "48" (FDI World Dental Federation)
    public SuperficieDental Superficie { get; private set; }
    public TipoHallazgo Tipo { get; private set; }
    public string Descripcion { get; private set; } // Ej: "Caries profunda", "Resina compuesta"

    private HallazgoOdontologico() { } // EF Core

    internal HallazgoOdontologico(string pieza, SuperficieDental superficie, TipoHallazgo tipo, string descripcion)
    {
        NomenclaturaPieza = pieza;
        Superficie = superficie;
        Tipo = tipo;
        Descripcion = descripcion;
    }
}
