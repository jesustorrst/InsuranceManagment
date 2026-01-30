using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public abstract class BaseEntity
    {
        public DateTime FechaCreacion { get; set; }
        public int CreadoPor { get; set; }

        public DateTime? FechaActualizacion { get; set; }
        public int? ActualizadoPor { get; set; }

        public bool Eliminado { get; set; }
    }

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

    public class Rol
    {
        public int IdRol { get; set; }

        public string Nombre { get; set; } = null!; 

        public string? Descripcion { get; set; }

        public bool Activo { get; set; }

        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }

    public class Usuario : BaseEntity
    {
        public int IdUsuario { get; set; }

        public int? IdCliente { get; set; }
        public int IdRol { get; set; }

        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;

        public bool Activo { get; set; }
        
        public Cliente? Cliente { get; set; }
        public Rol Rol { get; set; } = null!;
    }

    public class TipoPoliza
    {
        public int IdTipoPoliza { get; set; }

        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }

        public bool Activo { get; set; }

        public ICollection<Poliza> Polizas { get; set; } = new List<Poliza>();
    }

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
