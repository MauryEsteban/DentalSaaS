using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using DentalSaaS.Application.Citas.Commands;
using DentalSaaS.Application.Common.Interfaces;
using DentalSaaS.Domain.Entities;
using MockQueryable.Moq;

namespace DentalSaaS.Application.UnitTests.Citas.Commands;

public class CrearCitaHandlerTests
{
    [Fact]
    public async Task Handle_DebeLanzarExcepcion_CuandoCitasSeSolapan()
    {
        var odontologoId = Guid.NewGuid();
        var request = new CrearCitaCommand(Guid.NewGuid(), odontologoId, 
            new DateTime(2026, 4, 30, 10, 0, 0), 
            new DateTime(2026, 4, 30, 11, 0, 0));

        var citaExistente = new Cita(Guid.NewGuid(), odontologoId, 
            new DateTime(2026, 4, 30, 10, 30, 0), 
            new DateTime(2026, 4, 30, 11, 30, 0));

        var citasData = new List<Cita> { citaExistente };
        var mockDbSet = citasData.BuildMockDbSet();

        var mockContext = new Mock<IApplicationDbContext>();
        mockContext.Setup(c => c.Citas).Returns(mockDbSet.Object);

        var handler = new CrearCitaHandler(mockContext.Object);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => handler.Handle(request, CancellationToken.None));

        Assert.Contains("solapa con este bloque horario", exception.Message);
    }
}
