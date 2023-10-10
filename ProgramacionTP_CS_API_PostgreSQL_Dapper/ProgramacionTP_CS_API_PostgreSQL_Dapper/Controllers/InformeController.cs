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
    }
}
