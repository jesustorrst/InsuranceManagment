using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Entities;

namespace Data.DAL
{
    public class PolizaDAL
    {
        private readonly InsuranceManagmentDbContext _context;

        public PolizaDAL(InsuranceManagmentDbContext context)
        {
            _context = context;
        }

        public async Task<Result<int>> Add(Poliza poliza)
        {
            var result = new Result<int>();
            try
            {
                _context.Polizas.Add(poliza);
                int filasAfectadas = await _context.SaveChangesAsync();

                if (filasAfectadas > 0)
                {
                    result.Object = poliza.IdPoliza;
                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "No se pudo insertar la póliza.";
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

        public async Task<Result<Poliza>> GetByIdCliente(int idCliente)
        {
            var result = new Result<Poliza>();
            try
            {
                result.Objects = await _context.Polizas
                    .Include(p => p.TipoPoliza) 
                    .Where(p => p.IdCliente == idCliente && !p.Eliminado)
                    .ToListAsync();

                if (result.Objects != null && result.Objects.Any())
                {
                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "No se encontraron pólizas para este cliente.";
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public async Task<Result<Poliza>> GetById(int idPoliza)
        {
            var result = new Result<Poliza>();
            try
            {
                var poliza = await _context.Polizas
                    .FirstOrDefaultAsync(p => p.IdPoliza == idPoliza && !p.Eliminado);

                if (poliza != null)
                {
                    result.Object = poliza;
                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "Póliza no encontrada.";
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public async Task<Result<bool>> Update(Poliza poliza)
        {
            var result = new Result<bool>();
            try
            {
                _context.Entry(poliza).State = EntityState.Modified;
                int filasAfectadas = await _context.SaveChangesAsync();
                result.Correct = filasAfectadas > 0;
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