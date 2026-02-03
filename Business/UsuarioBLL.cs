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
            var entidad = new Models.Entities.Usuario
            {
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),

                IdRol = dto.IdRol,
                IdCliente = dto.IdCliente,

                Activo = true
            };

            return await _usuarioDAL.Add(entidad);
        }

        


        public async Task<Result<AuthResponse>> Login(LoginDTO loginDto)
        {
            var result = new Result<AuthResponse>();
            try
            {
                var resultDAL = await _usuarioDAL.GetByEmail(loginDto.Email);

                if (resultDAL.Correct && resultDAL.Object != null)
                {
                    var usuario = resultDAL.Object;

                    bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, usuario.PasswordHash);

                    if (isPasswordValid)
                    {
                        result.Correct = true;
                        result.Object = new AuthResponse
                        {
                            Email = usuario.Email,
                            IdCliente = usuario.IdCliente,
                            Rol = usuario.IdRol.ToString(),
                            Token = "" 
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
