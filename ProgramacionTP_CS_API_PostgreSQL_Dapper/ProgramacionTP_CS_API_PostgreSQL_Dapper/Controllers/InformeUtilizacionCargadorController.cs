using ProgramacionTB_CS_API_PostgreSQL_Dapper.Helpers;
using ProgramacionTB_CS_API_PostgreSQL_Dapper.Models;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Services;
using Microsoft.AspNetCore.Mvc;

namespace ProgramacionTP_CS_API_PostgreSQL_Dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InformeUtilizacionCargadorController : Controller
    {
        private readonly InformeUtilizacionCargadorService _informeUtilizacionCargadorService;

        public InformeUtilizacionCargadorController(InformeUtilizacionCargadorService informeUtilizacionCargadorService)
        {
            _informeUtilizacionCargadorService = informeUtilizacionCargadorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetInformeUtilizacionAsync()
        {
            var elResumen = await _informeUtilizacionCargadorService
                .GetInformeUtilizacionAsync();

            return Ok(elResumen);
        }
    }
}