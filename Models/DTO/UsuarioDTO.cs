using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class UsuarioDTO
    {
        public int IdUsuario { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } 
        public int? IdCliente { get; set; } 
        public int IdRol { get; set; }
    }
}
