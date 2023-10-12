using ProgramacionTB_CS_API_PostgreSQL_Dapper.Helpers;
using ProgramacionTB_CS_API_PostgreSQL_Dapper.Models;
using ProgramacionTB_CS_API_PostgreSQL_Dapper.Services;
using Microsoft.AspNetCore.Mvc;

namespace ProgramacionTP_CS_API_PostgreSQL_Dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InformeHoraController : Controller
    {
        private readonly InformeHoraService _informeHoraService;

        public InformeHoraController(InformeHoraService informeHoraService)
        {
            _informeHoraService = informeHoraService;
        }

        [HttpGet("{hora:int}")]
        public async Task<IActionResult> GetInformeHoraAsync(int hora)
        {
            try
            {
                var unInformeHora = await _informeHoraService
                    .GetInformeHoraAsync(hora);
                return Ok(unInformeHora);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }
    }
}