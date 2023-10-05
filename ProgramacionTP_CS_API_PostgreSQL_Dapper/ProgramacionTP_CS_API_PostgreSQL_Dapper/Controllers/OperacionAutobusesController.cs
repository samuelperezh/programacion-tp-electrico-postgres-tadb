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

        [HttpGet("{operacionAutobus_id:int}")]
        public async Task<IActionResult> GetByIdAsync(int operacionAutobus_id)
        {
            try
            {
                var unaOperacionAutobus = await _operacionAutobusService
                    .GetByIdAsync(operacionAutobus_id);
                return Ok(unaOperacionAutobus);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
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

        [HttpPut("{operacionAutobus_id:int}")]
        public async Task<IActionResult> UpdateAsync(int operacionAutobus_id, OperacionAutobus unaOperacionAutobus)
        {
            try
            {
                var operacionAutobusActualizado = await _operacionAutobusService
                    .UpdateAsync(operacionAutobus_id, unaOperacionAutobus);

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

        [HttpDelete("{operacionAutobus_id:int}")]
        public async Task<IActionResult> DeleteAsync(int operacionAutobus_id)
        {
            try
            {
                await _operacionAutobusService
                    .DeleteAsync(operacionAutobus_id);

                return Ok($"OperacionAutobus {operacionAutobus_id} fue eliminada");

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
