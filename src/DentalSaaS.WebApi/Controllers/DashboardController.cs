using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DentalSaaS.Application.Dashboard.Queries;
using DentalSaaS.Application.Common.Models;

namespace DentalSaaS.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
// [Authorize] -- Descomentar cuando el pipeline de JWT esté 100% probado en Swagger
public class DashboardController(IMediator mediator) : ControllerBase
{
    [HttpGet("resumen")]
    [ProducesResponseType(typeof(DashboardSummaryDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetResumen()
    {
        var query = new GetDashboardSummaryQuery();
        var result = await mediator.Send(query);

        return Ok(result);
    }
}