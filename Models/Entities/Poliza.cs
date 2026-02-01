using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class Poliza : BaseEntity
    {
        public int IdPoliza { get; set; }

        public int IdCliente { get; set; }
        public int IdTipoPoliza { get; set; }

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public decimal MontoAsegurado { get; set; }

        public string Estado { get; set; } = null!;

        public Cliente Cliente { get; set; } = null!;
        public TipoPoliza TipoPoliza { get; set; } = null!;
    }
}
