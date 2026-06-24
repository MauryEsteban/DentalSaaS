using System.Data;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using DentalSaaS.Application.Common.Interfaces;
using DentalSaaS.Application.Common.Models;

namespace DentalSaaS.Application.Dashboard.Queries;

public class GetDashboardSummaryHandler(
    IConfiguration configuration,
    ITenantService tenantService) : IRequestHandler<GetDashboardSummaryQuery, DashboardSummaryDto>
{
    public async Task<DashboardSummaryDto> Handle(GetDashboardSummaryQuery request, CancellationToken cancellationToken)
    {
        var tenantId = tenantService.TenantId;
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        // Obtenemos inicio y fin del día actual (UTC o local dependiendo de tu regla de negocio)
        var hoyInicio = DateTime.Today;
        var hoyFin = hoyInicio.AddDays(1).AddTicks(-1);

        const string sql = """
            -- 1. KPIs Generales
            SELECT 
                COUNT(Id) AS TotalCitasHoy,
                SUM(CASE WHEN Estado = 0 THEN 1 ELSE 0 END) AS CitasPendientes -- 0 = Pendiente
            FROM Citas
            WHERE TenantId = @TenantId 
              AND FechaInicio >= @HoyInicio AND FechaInicio <= @HoyFin
              AND Estado != 2; -- 2 = Cancelada

            -- 2. Ingresos del Día
            SELECT ISNULL(SUM(co.Monto), 0)
            FROM Cobros co
            INNER JOIN Citas ci ON co.CitaId = ci.Id
            WHERE co.TenantId = @TenantId
              AND ci.FechaInicio >= @HoyInicio AND ci.FechaInicio <= @HoyFin;

            -- 3. Próximas Citas (Agenda)
            -- Nota: Hardcodeamos 'Paciente Temporal' hasta que modelemos la tabla Pacientes
            SELECT TOP 10
                Id AS CitaId,
                'Paciente Temporal' AS NombrePaciente, 
                FechaInicio,
                CASE Estado 
                    WHEN 0 THEN 'Pendiente'
                    WHEN 1 THEN 'Confirmada'
                    WHEN 3 THEN 'Realizada'
                    ELSE 'Desconocido'
                END AS EstadoConsulta
            FROM Citas
            WHERE TenantId = @TenantId 
              AND FechaInicio >= @HoyInicio 
              AND Estado NOT IN (2, 3) -- No mostrar canceladas ni ya realizadas en la cola
            ORDER BY FechaInicio ASC;
            """;

        using IDbConnection db = new SqlConnection(connectionString);

        // QueryMultiple permite ejecutar los 3 SELECTs en un solo viaje de red (Eficiencia extrema)
        using var multi = await db.QueryMultipleAsync(sql, new { TenantId = tenantId, HoyInicio = hoyInicio, HoyFin = hoyFin });

        var kpis = await multi.ReadSingleAsync<(int TotalCitasHoy, int CitasPendientes)>();
        var ingresosHoy = await multi.ReadSingleAsync<decimal>();
        var proximasCitas = (await multi.ReadAsync<CitaResumenDto>()).ToList();

        return new DashboardSummaryDto(
            kpis.TotalCitasHoy,
            kpis.CitasPendientes,
            ingresosHoy,
            proximasCitas.AsReadOnly()
        );
    }
}