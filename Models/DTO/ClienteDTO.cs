using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class ClienteDTO
    {
        [Required(ErrorMessage = "El Número de identificación es obligatorio")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "El Número de identificación debe ser de exactamente 10 dígitos numéricos.")]
        public string NumeroIdentificacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El Nombre es obligatorio")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El Nombre no debe contener números ni caracteres especiales.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El Apellido Paterno es obligatorio")]
        public string ApellidoPaterno { get; set; } = string.Empty;

        public string? ApellidoMaterno { get; set; }

        [Required(ErrorMessage = "El Email es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El Teléfono es obligatorio")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "El teléfono debe ser de exactamente 10 dígitos numéricos.")]
        public string? Telefono { get; set; }
    }

}
