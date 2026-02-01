using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class Usuario
    {
        public int IdUsuario { get; set; }

        public int? IdCliente { get; set; }
        public int IdRol { get; set; }

        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;

        public bool Activo { get; set; }

        public virtual Cliente? Cliente { get; set; }
        public virtual Rol Rol { get; set; } = null!;
    }
}
