using ProgramacionTB_CS_API_PostgreSQL_Dapper.Helpers;
using ProgramacionTB_CS_API_PostgreSQL_Dapper.Models;
using ProgramacionTB_CS_API_PostgreSQL_Dapper.Services;
using Microsoft.AspNetCore.Mvc;

namespace ProgramacionTP_CS_API_PostgreSQL_Dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperacionAutobusesController : Controller
    {
        private readonly OperacionAutobusService _operacionAutobusService;

        public OperacionAutobusesController(OperacionAutobusService operacionAutobusService)
        {
            _operacionAutobusService = operacionAutobusService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var losOperacionAutobuses = await _operacionAutobusService
                .GetAllAsync();

            return Ok(losOperacionAutobuses);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(OperacionAutobus unaOperacionAutobus)
        {
            try
            {
                var operacionAutobusCreado = await _operacionAutobusService
                    .CreateAsync(unaOperacionAutobus);

                return Ok(operacionAutobusCreado);
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

        [HttpPut("{autobus_id:int}/{horario_id:int}")]
        public async Task<IActionResult> UpdateAsync(int autobus_id, int horario_id, OperacionAutobus unaOperacionAutobus)
        {
            try
            {
                var operacionAutobusActualizado = await _operacionAutobusService
                    .UpdateAsync(autobus_id, horario_id, unaOperacionAutobus);

                return Ok(operacionAutobusActualizado);

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

        [HttpDelete("{autobus_id:int}/{horario_id:int}")]
        public async Task<IActionResult> DeleteAsync(int autobus_id, int horario_id)
        {
            try
            {
                await _operacionAutobusService
                    .DeleteAsync(autobus_id, horario_id);

                return Ok($"La operacion del autobus {autobus_id} en el horario {horario_id} fue eliminada");

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
