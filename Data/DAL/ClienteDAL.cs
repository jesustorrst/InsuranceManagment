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
    public class ClienteDAL
    {
        private readonly InsuranceManagmentDbContext _context;

        public ClienteDAL(InsuranceManagmentDbContext context)
        {
            _context = context;
        }

        public async Task<Result<int>> Add(Models.Entities.Cliente cliente)
        {
            var result = new Result<int>();
            try
            {
                bool existe = await _context.Clientes.AnyAsync(c => c.NumeroIdentificacion == cliente.NumeroIdentificacion && !c.Eliminado);

                if (existe)
                {
                    result.Correct = false;
                    result.ErrorMessage = "El número de identificación ya se encuentra registrado.";
                    return result;
                }

                _context.Clientes.Add(cliente);

                int filasAfectadas = await _context.SaveChangesAsync();

                if (filasAfectadas > 0)
                {
                    result.Object = cliente.IdCliente;
                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "No se pudo insertar el registro en la base de datos.";
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


        public async Task<Result<Models.Entities.Cliente>> GetAll()
        {
            var result = new Result<Models.Entities.Cliente>();
            try
            {

                result.Objects = await _context.Clientes.Where(c => !c.Eliminado).ToListAsync();

                if (result.Objects != null && result.Objects.Any())
                {
                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "No se encontraron clientes registrados.";
                }

                result.Correct = true;
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }


        public async Task<Result<Models.Entities.Cliente>> GetById(int idCliente)
        {
            var result = new Result<Models.Entities.Cliente>();
            try
            {
                var cliente = await _context.Clientes
                    .FirstOrDefaultAsync(c => c.IdCliente == idCliente && !c.Eliminado);

                if (cliente != null)
                {
                    result.Object = cliente; 
                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "El cliente solicitado no existe o fue eliminado.";
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

        public async Task<Result<bool>> Update(Models.Entities.Cliente cliente)
        {
            var result = new Result<bool>();
            try
            {
                _context.Clientes.Update(cliente);
                int filasAfectadas = await _context.SaveChangesAsync();

                if (filasAfectadas > 0)
                {
                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "No se pudo actualizar el registro en la base de datos.";
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }




    }
}
