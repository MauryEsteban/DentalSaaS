using MediatR;
using DentalSaaS.Application.Common.Models;

namespace DentalSaaS.Application.Citas.Queries;

public record ObtenerCitasPorRangoQuery(DateTime Desde, DateTime Hasta) : IRequest<IReadOnlyCollection<CitaCalendarioDto>>;