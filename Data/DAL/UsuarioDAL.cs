using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Entities;

namespace Data.DAL
{
    public class UsuarioDAL
    {
        private readonly InsuranceManagmentDbContext _context;
        public UsuarioDAL(InsuranceManagmentDbContext context)
        {
            _context = context;
        }


        public async Task<Result<Usuario>> GetByEmail(string email)
        {
            var result = new Result<Usuario>();
            try
            {
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

                if (usuario != null)
                {
                    result.Object = usuario;
                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "Usuario no encontrado.";
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

        public async Task<Result<int>> Add(Usuario usuario)
        {
            var result = new Result<int>();
            try
            {
                _context.Usuarios.Add(usuario);
                int filasAfectadas = await _context.SaveChangesAsync();

                if (filasAfectadas > 0)
                {
                    result.Object = usuario.IdUsuario;
                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "No se pudo insertar el usuario.";
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
