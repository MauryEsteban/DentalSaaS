using DentalSaaS.Domain.Common;

namespace DentalSaaS.Domain.BoundedContexts.Clinical.Entities;

public class EvolucionDraft : BaseEntity
{
    public Guid FichaClinicaId { get; private set; }
    public Guid OdontologoId { get; private set; }
    
    // Aquí guardamos TODO el estado de la UI (Blazor) serializado de golpe.
    // En SQL Server se mapeará a un NVARCHAR(MAX) o tipo JSON nativo.
    public string PayloadJson { get; private set; } 
    
    public DateTime UltimaModificacion { get; private set; }

    private EvolucionDraft() { } // EF Core

    public EvolucionDraft(Guid fichaClinicaId, Guid odontologoId, string payloadJson)
    {
        FichaClinicaId = fichaClinicaId;
        OdontologoId = odontologoId;
        PayloadJson = payloadJson;
        UltimaModificacion = DateTime.UtcNow;
    }

    public void ActualizarPayload(string nuevoPayloadJson)
    {
        PayloadJson = nuevoPayloadJson;
        UltimaModificacion = DateTime.UtcNow;
    }
}
