using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class TipoPoliza
    {
        public int IdTipoPoliza { get; set; }

        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }

        public bool Activo { get; set; }

        public ICollection<Poliza> Polizas { get; set; } = new List<Poliza>();
    }
}
