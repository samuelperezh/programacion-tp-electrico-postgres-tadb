using ProgramacionTB_CS_API_PostgreSQL_Dapper.Helpers;
using ProgramacionTB_CS_API_PostgreSQL_Dapper.Models;
using ProgramacionTB_CS_API_PostgreSQL_Dapper.Services;
using Microsoft.AspNetCore.Mvc;

namespace ProgramacionTP_CS_API_PostgreSQL_Dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutobusesController : Controller
    {
        private readonly AutobusService _autobusService;

        public AutobusesController(AutobusService autobusService)
        {
            _autobusService = autobusService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var losAutobuses = await _autobusService
                .GetAllAsync();

            return Ok(losAutobuses);
        }

        [HttpGet("{autobus_id:int}")]
        public async Task<IActionResult> GetByIdAsync(int autobus_id)
        {
            try
            {
                var unAutobus = await _autobusService
                    .GetByIdAsync(autobus_id);
                return Ok(unAutobus);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpPost("{autobus_id:int}")]
        public async Task<IActionResult> CreateAsync(Autobus unAutobus)
        {
            try
            {
                var autobusCreado = await _autobusService
                    .CreateAsync(unAutobus);

                return Ok(autobusCreado);
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

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(int autobus_id, Autobus unAutobus)
        {
            try
            {
                var autobusActualizado = await _autobusService
                    .UpdateAsync(autobus_id, unAutobus);

                return Ok(autobusActualizado);

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

        [HttpDelete("{autobus_id:int}")]
        public async Task<IActionResult> DeleteAsync(int autobus_id)
        {
            try
            {
                await _autobusService
                    .DeleteAsync(autobus_id);

                return Ok($"Autobus {autobus_id} fue eliminada");

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
