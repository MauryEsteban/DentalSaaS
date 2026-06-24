using MediatR;
using Microsoft.AspNetCore.Mvc;
using DentalSaaS.Application.BoundedContexts.Clinical.Commands;

namespace DentalSaaS.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class FichasController(IMediator mediator) : ControllerBase
{
    [HttpPost("test-patient")]
    public async Task<IActionResult> GenerarPacientePrueba()
    {
        var command = new CrearPacientePruebaCommand();
        var fichaId = await mediator.Send(command);
        
        // Retornamos el ID de la ficha creada
        return Ok(fichaId);
    }
}
