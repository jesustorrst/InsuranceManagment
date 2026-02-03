using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.DAL;
using Microsoft.Extensions.Configuration;
using Models;
using Models.DTO;
using Models.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace Business
{
    public class UsuarioBLL
    {
        private readonly Data.DAL.UsuarioDAL _usuarioDAL;

        public UsuarioBLL(Data.DAL.UsuarioDAL usuarioDAL)
        {
            _usuarioDAL = usuarioDAL;
        }

        public async Task<Result<int>> Add(Models.DTO.UsuarioDTO dto)
        {
            // 1. Mapeo y Hasheo de seguridad
            // Convertimos el password plano en un Hash antes de que toque la base de datos
            var entidad = new Models.Entities.Usuario
            {
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),

                IdRol = dto.IdRol,
                IdCliente = dto.IdCliente,

                // Campos de auditoría similares a tus otros modelos
                //FechaCreacion = DateTime.Now,
                Activo = true
            };

            // 2. Llamada al DAL que ya estructuramos
            return await _usuarioDAL.Add(entidad);
        }

        


        public async Task<Result<AuthResponse>> Login(LoginDTO loginDto)
        {
            var result = new Result<AuthResponse>();
            try
            {
                // 1. Buscamos el usuario en la DB a través del DAL
                var resultDAL = await _usuarioDAL.GetByEmail(loginDto.Email);

                if (resultDAL.Correct && resultDAL.Object != null)
                {
                    var usuario = resultDAL.Object;

                    // 2. Comparamos la contraseña (Texto Plano vs Hash)
                    // Esta función de BCrypt es la que hace la magia
                    bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, usuario.PasswordHash);

                    if (isPasswordValid)
                    {
                        result.Correct = true;
                        result.Object = new AuthResponse
                        {
                            Email = usuario.Email,
                            IdCliente = usuario.IdCliente,
                            Rol = usuario.IdRol.ToString(),
                            Token = "" // El Token lo llenaremos en el Controller
                        };
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Credenciales inválidas.";
                    }
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "El usuario no existe.";
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }


    }
}
