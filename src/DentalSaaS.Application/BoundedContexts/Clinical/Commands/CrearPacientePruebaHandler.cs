using MediatR;
using DentalSaaS.Application.Common.Interfaces;
using DentalSaaS.Domain.BoundedContexts.Clinical.Entities;

namespace DentalSaaS.Application.BoundedContexts.Clinical.Commands;

public class CrearPacientePruebaHandler(IApplicationDbContext context) : IRequestHandler<CrearPacientePruebaCommand, Guid>
{
    public async Task<Guid> Handle(CrearPacientePruebaCommand request, CancellationToken cancellationToken)
    {
        // 1. Creamos al paciente de prueba
        var paciente = new Paciente(
            rut: "11.111.111-1",
            nombres: "Juan Temporal",
            apellidos: "Pérez Prueba",
            fechaNacimiento: new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            telefono: "+56912345678",
            email: "juan.temporal@dentalsaas.local"
        );
        
        context.Pacientes.Add(paciente);

        // 2. Le creamos su Ficha Clínica vacía (Aggregate Root)
        var ficha = new FichaClinica(
            pacienteId: paciente.Id,
            antecedentesMedicos: "Paciente de prueba generado automáticamente. Sin alergias conocidas."
        );

        context.FichasClinicas.Add(ficha);

        // 3. Guardamos en la BD (El Interceptor le inyectará el TenantId automáticamente a ambos registros)
        await context.SaveChangesAsync(cancellationToken);

        return ficha.Id;
    }
}
