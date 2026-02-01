using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public abstract class BaseEntity
    {
        public DateTime FechaCreacion { get; set; }
        public int CreadoPor { get; set; }

        public DateTime? FechaActualizacion { get; set; }
        public int? ActualizadoPor { get; set; }

        public bool Eliminado { get; set; }


        public virtual Usuario UsuarioCreador { get; set; } = null!;
        public virtual Usuario? UsuarioActualizador { get; set; }
    }
}
