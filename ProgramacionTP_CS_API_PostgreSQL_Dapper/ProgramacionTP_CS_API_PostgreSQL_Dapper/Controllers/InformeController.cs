using ProgramacionTB_CS_API_PostgreSQL_Dapper.Helpers;
using ProgramacionTB_CS_API_PostgreSQL_Dapper.Models;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Services;
using Microsoft.AspNetCore.Mvc;

namespace ProgramacionTP_CS_API_PostgreSQL_Dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InformeController : Controller
    {
        private readonly InformeService _informeService;

        public InformeController(InformeService resumenService)
        {
            _informeService = resumenService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var elResumen = await _informeService
                .GetAllAsync();

            return Ok(elResumen);
        }

        [HttpGet("{horario_id:int}")]
        public async Task<IActionResult> GetByIdAsync(int horario_id)
        {
            try
            {
                var unInforme = await _informeService
                    .GetByIdAsync(horario_id);
                return Ok(unInforme);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }
    }
}
