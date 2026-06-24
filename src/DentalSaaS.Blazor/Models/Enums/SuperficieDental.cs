namespace DentalSaaS.Blazor.Models.Enums;

[System.Flags] // Usamos Flags para poder combinar (Ej: Oclusal | Mesial | Distal = OMD)
public enum SuperficieDental 
{ 
    Ninguna = 0,
    Oclusal = 1,
    Mesial = 2,
    Distal = 4,
    Vestibular = 8,
    PalatinoLingual = 16,
    Cervical = 32,
    CoronaCompleta = 64
}

public enum ProfundidadCaries
{
    NoAplica,
    Esmalte,
    Dentina,
    Pulpar
}
