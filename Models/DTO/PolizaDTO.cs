using System;

namespace Models.DTO
{
    public class PolizaDTO
    {
        public int IdPoliza { get; set; }
        public int IdCliente { get; set; } // El dueño de la póliza
        public int IdTipoPoliza { get; set; } // La relación con la otra entidad

        public decimal MontoAsegurado { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public string Estado { get; set; } = string.Empty; // "Activa" o "Cancelada"

        public string? NombreTipoPoliza { get; set; }
        public string? DescripcionTipoPoliza { get; set; }
    }
}