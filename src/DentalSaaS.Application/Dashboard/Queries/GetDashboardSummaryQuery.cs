using MediatR;
using DentalSaaS.Application.Common.Models;

namespace DentalSaaS.Application.Dashboard.Queries;

public record GetDashboardSummaryQuery : IRequest<DashboardSummaryDto>;