namespace DentalSaaS.Application.Common.Models;

// Usamos record para inmutabilidad y eficiencia en memoria
public record DashboardSummaryDto(
    int TotalCitasHoy,
    int CitasPendientes,
    decimal IngresosHoy,
    IReadOnlyCollection<CitaResumenDto> ProximasCitas
);

public record CitaResumenDto(
    Guid CitaId,
    string NombrePaciente,
    DateTime FechaInicio,
    string EstadoConsulta
);