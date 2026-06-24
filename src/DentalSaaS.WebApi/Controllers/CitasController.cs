using DentalSaaS.Application.Citas.Commands;
using DentalSaaS.Application.Citas.Queries;
using DentalSaaS.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalSaaS.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
//[Authorize]
public class CitasController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CrearCita([FromBody] CrearCitaCommand command)
    {
        var citaId = await mediator.Send(command);
        
        return CreatedAtAction(nameof(ObtenerCita), new { id = citaId }, new { Id = citaId });
    }

    [HttpGet("{id}")]
    public IActionResult ObtenerCita(Guid id)
    {
        return Ok();
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<CitaCalendarioDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerCitas([FromQuery] DateTime desde, [FromQuery] DateTime hasta)
    {
        var query = new ObtenerCitasPorRangoQuery(desde, hasta);
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpPut("{id}/reprogramar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ReprogramarCita(Guid id, [FromBody] ReprogramarCitaCommand command)
    {
        if (id != command.CitaId) return BadRequest("Discordancia de IDs.");

        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CancelarCita(Guid id)
    {
        var command = new CancelarCitaCommand(id);
        await mediator.Send(command);
        return NoContent();
    }
}
