using System.Data;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using DentalSaaS.Application.Common.Interfaces;
using DentalSaaS.Application.Common.Models;

namespace DentalSaaS.Application.Citas.Queries;

public class ObtenerCitasPorRangoHandler(
    IConfiguration configuration,
    ITenantService tenantService) : IRequestHandler<ObtenerCitasPorRangoQuery, IReadOnlyCollection<CitaCalendarioDto>>
{
    public async Task<IReadOnlyCollection<CitaCalendarioDto>> Handle(ObtenerCitasPorRangoQuery request, CancellationToken cancellationToken)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        const string sql = """
            SELECT 
                Id,
                PacienteId,
                'Paciente Temporal' AS NombrePaciente, -- Hardcodeado hasta modelar Pacientes
                OdontologoId,
                FechaInicio,
                FechaFin,
                CASE Estado 
                    WHEN 0 THEN 'Pendiente'
                    WHEN 1 THEN 'Confirmada'
                    WHEN 2 THEN 'Cancelada'
                    WHEN 3 THEN 'Realizada'
                    ELSE 'Desconocido'
                END AS Estado
            FROM Citas
            WHERE TenantId = @TenantId
              AND FechaInicio >= @Desde 
              AND FechaInicio <= @Hasta
                AND Estado != 2 
            ORDER BY FechaInicio ASC;
            """;

        using IDbConnection db = new SqlConnection(connectionString);
        var citas = await db.QueryAsync<CitaCalendarioDto>(sql, new
        {
            TenantId = tenantService.TenantId,
            Desde = request.Desde,
            Hasta = request.Hasta
        });

        return citas.ToList().AsReadOnly();
    }
}