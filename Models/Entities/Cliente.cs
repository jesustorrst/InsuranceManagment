using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class Cliente : BaseEntity
    {
        public int IdCliente { get; set; }

        public string NumeroIdentificacion { get; set; } = null!;

        public string Nombre { get; set; } = null!;
        public string ApellidoPaterno { get; set; } = null!;
        public string? ApellidoMaterno { get; set; }

        public string Email { get; set; } = null!;
        public string? Telefono { get; set; }


        public byte[]? Foto { get; set; }

        // Navegación
        public ICollection<Poliza> Polizas { get; set; } = new List<Poliza>();
    }
}
