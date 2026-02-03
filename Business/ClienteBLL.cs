using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Models;
using Models.DTO;
using Models.Entities;

namespace Business
{
    public class ClienteBLL
    {
        private readonly Data.DAL.ClienteDAL _clienteDAL;

        public ClienteBLL(Data.DAL.ClienteDAL clienteRepo)
        {
            _clienteDAL = clienteRepo;
        }
        public async Task<Result<int>> Add(Models.DTO.ClienteDTO dto)
        {
            // Mapeo del DTO a la Entidad completa

            var entidad = new Models.Entities.Cliente
            {
                NumeroIdentificacion = dto.NumeroIdentificacion,
                Nombre = dto.Nombre,
                ApellidoPaterno = dto.ApellidoPaterno,
                ApellidoMaterno = dto.ApellidoMaterno,
                Email = dto.Email,
                Telefono = dto.Telefono,

                FechaCreacion = DateTime.Now,
                CreadoPor = 5, //usuario logueado
                Eliminado = false
            };

            return await _clienteDAL.Add(entidad);
        }


        public async Task<Result<Cliente>> GetAll()
        {
            return await _clienteDAL.GetAll();
        }

        public async Task<Result<Cliente>> GetById(int id)
        {
            if (id <= 0)
            {
                return new Result<Cliente> { Correct = false, ErrorMessage = "Id no válido." };
            }
            return await _clienteDAL.GetById(id);
        }

        public async Task<Result<bool>> Update(int id, ClienteDTO dto)
        {

            var existingResult = await _clienteDAL.GetById(id);

            if (!existingResult.Correct || existingResult.Object == null)
            {
                return new Result<bool> { Correct = false, ErrorMessage = "El cliente no existe." };
            }

            var entidad = existingResult.Object;
            entidad.NumeroIdentificacion = dto.NumeroIdentificacion;
            entidad.Nombre = dto.Nombre;
            entidad.ApellidoPaterno = dto.ApellidoPaterno;
            entidad.ApellidoMaterno = dto.ApellidoMaterno;
            entidad.Email = dto.Email;
            entidad.Telefono = dto.Telefono;

            entidad.FechaActualizacion = DateTime.Now;
            entidad.ActualizadoPor = 5; 

            return await _clienteDAL.Update(entidad);
        }

        public async Task<Result<bool>> Delete(int id)
        {
            var existingResult = await _clienteDAL.GetById(id);

            if (!existingResult.Correct || existingResult.Object == null)
            {
                return new Result<bool> { Correct = false, ErrorMessage = "No existe un cliente con el id seleccionado." };
            }

            var entidad = existingResult.Object;
            entidad.Eliminado = true; 
            entidad.FechaActualizacion = DateTime.Now;
            entidad.ActualizadoPor = 5;

            return await _clienteDAL.Update(entidad);
        }



    }

}
