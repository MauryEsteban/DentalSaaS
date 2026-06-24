using DentalSaaS.Domain.Common;
using DentalSaaS.Domain.Enums;

namespace DentalSaaS.Domain.Entities;

public class Cita : BaseEntity
{
    public Guid PacienteId { get; private set; }
    public Guid OdontologoId { get; private set; }
    public DateTime FechaInicio { get; private set; }
    public DateTime FechaFin { get; private set; }
    public EstadoCita Estado { get; private set; }
    public Cobro? Cobro { get; private set; }
    
    private readonly List<string> _notas = [];
    public IReadOnlyCollection<string> Notas => _notas.AsReadOnly();

    private Cita() { }

    public Cita(Guid pacienteId, Guid odontologoId, DateTime fechaInicio, DateTime fechaFin)
    {
        PacienteId = pacienteId;
        OdontologoId = odontologoId;
        FechaInicio = fechaInicio;
        FechaFin = fechaFin;
        Estado = EstadoCita.Pendiente;
    }

    public void AgregarNotas(params ReadOnlySpan<string> nuevasNotas)
    {
        foreach (var nota in nuevasNotas)
        {
            _notas.Add(nota);
        }
    }

    public void MarcarComoRealizada(decimal montoCobro)
    {
        if (Estado == EstadoCita.Cancelada)
            throw new InvalidOperationException("No se puede realizar una cita cancelada.");
            
        Estado = EstadoCita.Realizada;
        Cobro = new Cobro(Id, montoCobro);
    }

    public void Reprogramar(DateTime nuevaFechaInicio, DateTime nuevaFechaFin)
    {
        if (Estado == EstadoCita.Realizada || Estado == EstadoCita.Cancelada)
            throw new InvalidOperationException("No se puede reprogramar una cita que ya fue completada o cancelada.");

        FechaInicio = nuevaFechaInicio;
        FechaFin = nuevaFechaFin;
    }

    // NUEVO MÉTODO PARA ENCAPSULAR LA CANCELACIÓN
    public void Cancelar()
    {
        if (Estado == EstadoCita.Realizada)
            throw new InvalidOperationException("No se puede cancelar una cita que ya fue realizada y cobrada.");
            
        Estado = EstadoCita.Cancelada;
    }
}
