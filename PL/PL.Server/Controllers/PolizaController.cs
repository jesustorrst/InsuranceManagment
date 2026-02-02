using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTO; // Asegúrate de tener PolizaDTO aquí
using System.Threading.Tasks;

namespace PL.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolizaController : ControllerBase
    {
        private readonly Business.PolizaBLL _polizaService;

        public PolizaController(Business.PolizaBLL polizaService)
        {
            _polizaService = polizaService;
        }

        [HttpGet("GetByIdCliente/{idCliente}")]
        public async Task<IActionResult> GetByIdCliente(int idCliente)
        {
            var result = await _polizaService.GetByIdCliente(idCliente);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _polizaService.GetById(id);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PolizaDTO polizaDTO)
        {
            if (polizaDTO == null)
            {
                return BadRequest("Los datos de la póliza son requeridos.");
            }

            var result = await _polizaService.Add(polizaDTO);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] PolizaDTO polizaDTO)
        {
            if (polizaDTO == null)
            {
                return BadRequest("Los datos para la actualización de la poliza son requeridos.");
            }

            var result = await _polizaService.Update(id, polizaDTO);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _polizaService.Delete(id);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}