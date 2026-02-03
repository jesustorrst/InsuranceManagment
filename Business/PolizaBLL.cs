using System;
using System.Threading.Tasks;
using Models;
using Models.DTO;
using Models.Entities;

namespace Business
{
    public class PolizaBLL
    {
        private readonly Data.DAL.PolizaDAL _polizaDAL;

        public PolizaBLL(Data.DAL.PolizaDAL polizaRepo)
        {
            _polizaDAL = polizaRepo;
        }

        public async Task<Result<Models.DTO.PolizaDTO>> GetByIdCliente(int idCliente)
        {
            var result = new Result<PolizaDTO>();

            var resultDAL = await _polizaDAL.GetByIdCliente(idCliente);

            if (resultDAL.Correct && resultDAL.Objects != null)
            {
                result.Objects = resultDAL.Objects.Select(p => new PolizaDTO
                {
                    IdPoliza = p.IdPoliza,
                    IdCliente = p.IdCliente,
                    IdTipoPoliza = p.IdTipoPoliza,
                    MontoAsegurado = p.MontoAsegurado,
                    FechaInicio = p.FechaInicio,
                    FechaFin = p.FechaFin,
                    Estado = p.Estado,
                    NombreTipoPoliza = p.TipoPoliza?.Nombre,
                    DescripcionTipoPoliza = p.TipoPoliza?.Descripcion
                }).ToList();
                result.Correct = true;
            }
            else
            {
                result.Correct = false;
                result.ErrorMessage = resultDAL.ErrorMessage;
            }
            return result;
        }

        public async Task<Result<int>> Add(PolizaDTO dto)
        {
            var entidad = new Poliza
            {
                IdCliente = dto.IdCliente,
                IdTipoPoliza = dto.IdTipoPoliza, 
                MontoAsegurado = dto.MontoAsegurado,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                Estado = dto.Estado,

                FechaCreacion = DateTime.Now,
                CreadoPor = 5,
                Eliminado = false
            };

            return await _polizaDAL.Add(entidad);
        }

       
        public async Task<Result<bool>> Update(int id, PolizaDTO dto)
        {
            var existingResult = await _polizaDAL.GetById(id);

            if (!existingResult.Correct || existingResult.Object == null)
            {
                return new Result<bool> { Correct = false, ErrorMessage = "La póliza no existe." };
            }

            var entidad = existingResult.Object;

            entidad.IdTipoPoliza = dto.IdTipoPoliza;
            entidad.MontoAsegurado = dto.MontoAsegurado;
            entidad.FechaInicio = dto.FechaInicio;
            entidad.FechaFin = dto.FechaFin;
            entidad.Estado = dto.Estado;

            entidad.FechaActualizacion = DateTime.Now;
            entidad.ActualizadoPor = 5;

            return await _polizaDAL.Update(entidad);
        }

        public async Task<Result<Poliza>> GetById(int id)
        {
            if (id <= 0)
            {
                return new Result<Poliza> { Correct = false, ErrorMessage = "Id de póliza no válido." };
            }
            return await _polizaDAL.GetById(id);
        }

        public async Task<Result<bool>> Delete(int id)
        {

            var existingResult = await _polizaDAL.GetById(id);

            if (!existingResult.Correct || existingResult.Object == null)
            {
                return new Result<bool> { Correct = false, ErrorMessage = "No existe una póliza con el id seleccionado." };
            }

            var entidad = existingResult.Object;

            entidad.Eliminado = true;
            entidad.FechaActualizacion = DateTime.Now;
            entidad.ActualizadoPor = 5;

            return await _polizaDAL.Update(entidad);
        }

    }
}