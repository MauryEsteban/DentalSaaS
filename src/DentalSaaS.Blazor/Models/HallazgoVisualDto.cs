using DentalSaaS.Blazor.Models.Enums;

namespace DentalSaaS.Blazor.Models;

public class HallazgoVisualDto
{
    public HerramientaClinica Tipo { get; set; }
    public SuperficieDental Superficies { get; set; } // Ahora puede ser múltiple por Flags
    public ProfundidadCaries Profundidad { get; set; } = ProfundidadCaries.NoAplica;
    public string Notas { get; set; } = string.Empty;
}
