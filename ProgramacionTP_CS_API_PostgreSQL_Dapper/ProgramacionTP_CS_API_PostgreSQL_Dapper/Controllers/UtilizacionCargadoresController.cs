using ProgramacionTB_CS_API_PostgreSQL_Dapper.Helpers;
using ProgramacionTB_CS_API_PostgreSQL_Dapper.Models;
using ProgramacionTB_CS_API_PostgreSQL_Dapper.Services;
using Microsoft.AspNetCore.Mvc;

namespace ProgramacionTP_CS_API_PostgreSQL_Dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilizacionCargadorController : Controller
    {
        private readonly UtilizacionCargadorService _utilizacionCargadorService;

        public UtilizacionCargadorController(UtilizacionCargadorService utilizacionCargadorService)
        {
            _utilizacionCargadorService = utilizacionCargadorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var lasUtilizacionesCargadores = await _utilizacionCargadorService
                .GetAllAsync();

            return Ok(lasUtilizacionesCargadores);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(UtilizacionCargador unaUtilizacionCargador)
        {
            try
            {
                var utilizacionCargadorCreado = await _utilizacionCargadorService
                    .CreateAsync(unaUtilizacionCargador);

                return Ok(utilizacionCargadorCreado);
            }
            catch (AppValidationException error)
            {
                return BadRequest($"Error de validación: {error.Message}");
            }
            catch (DbOperationException error)
            {
                return BadRequest($"Error de operacion en DB: {error.Message}");
            }
        }

        [HttpPut("{cargador_id:int}/{autobus_id:int}/{horario_id:int}")]
        public async Task<IActionResult> UpdateAsync(int cargador_id, int autobus_id, int horario_id, UtilizacionCargador unaUtilizacionCargador)
        {
            try
            {
                var utilizacionCargadorActualizado = await _utilizacionCargadorService
                    .UpdateAsync(cargador_id, autobus_id, horario_id, unaUtilizacionCargador);

                return Ok(utilizacionCargadorActualizado);

            }
            catch (AppValidationException error)
            {
                return BadRequest($"Error de validación: {error.Message}");
            }
            catch (DbOperationException error)
            {
                return BadRequest($"Error de operacion en DB: {error.Message}");
            }
        }

        [HttpDelete("{cargador_id:int}/{autobus_id:int}/{horario_id:int}")]
        public async Task<IActionResult> DeleteAsync(int cargador_id, int autobus_id, int horario_id)
        {
            try
            {
                await _utilizacionCargadorService
                    .DeleteAsync(cargador_id,autobus_id, horario_id);

                return Ok($"Utilización del cargador {cargador_id} con el autobus {autobus_id} y en el horario {horario_id} fue eliminada");

            }
            catch (AppValidationException error)
            {
                return BadRequest($"Error de validación: {error.Message}");
            }
            catch (DbOperationException error)
            {
                return BadRequest($"Error de operacion en DB: {error.Message}");
            }
        }
    }
}