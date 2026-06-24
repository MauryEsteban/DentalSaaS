using DentalSaaS.Domain.Common;

namespace DentalSaaS.Domain.BoundedContexts.Clinical.Entities;

public class FichaClinica : BaseEntity
{
    public Guid PacienteId { get; private set; }
    public string AntecedentesMedicos { get; private set; } // Alergias, enfermedades crónicas
    
    private readonly List<Evolucion> _evoluciones = [];
    public IReadOnlyCollection<Evolucion> Evoluciones => _evoluciones.AsReadOnly();

    private FichaClinica() { } // EF Core

    public FichaClinica(Guid pacienteId, string antecedentesMedicos = "")
    {
        PacienteId = pacienteId;
        AntecedentesMedicos = antecedentesMedicos;
    }

    public void ActualizarAntecedentes(string nuevosAntecedentes)
    {
        AntecedentesMedicos = nuevosAntecedentes;
    }

    public Evolucion AgregarEvolucion(Guid odontologoId, string notaClinica)
    {
        var nuevaEvolucion = new Evolucion(odontologoId, notaClinica);
        _evoluciones.Add(nuevaEvolucion);
        return nuevaEvolucion;
    }
}
