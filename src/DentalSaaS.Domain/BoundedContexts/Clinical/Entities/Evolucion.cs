using DentalSaaS.Domain.Common;

namespace DentalSaaS.Domain.BoundedContexts.Clinical.Entities;

public class Evolucion : BaseEntity
{
    public Guid FichaClinicaId { get; private set; }
    public Guid OdontologoId { get; private set; }
    public DateTime FechaAtencion { get; private set; }
    public string NotaClinica { get; private set; }

    private readonly List<HallazgoOdontologico> _hallazgos = [];
    public IReadOnlyCollection<HallazgoOdontologico> Hallazgos => _hallazgos.AsReadOnly();

    private Evolucion() { } // EF Core

    internal Evolucion(Guid odontologoId, string notaClinica)
    {
        OdontologoId = odontologoId;
        FechaAtencion = DateTime.UtcNow;
        NotaClinica = notaClinica;
    }

    public void RegistrarHallazgo(string pieza, Enums.SuperficieDental superficie, Enums.TipoHallazgo tipo, string descripcion)
    {
        _hallazgos.Add(new HallazgoOdontologico(pieza, superficie, tipo, descripcion));
    }
}
