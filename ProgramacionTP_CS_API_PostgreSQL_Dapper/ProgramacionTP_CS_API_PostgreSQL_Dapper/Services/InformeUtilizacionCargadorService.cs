using ProgramacionTB_CS_API_PostgreSQL_Dapper.Helpers;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Models;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Repositories;

namespace ProgramacionTP_CS_API_PostgreSQL_Dapper.Services
{
    public class InformeUtilizacionCargadorService
    {
        private readonly IInformeUtilizacionCargadorRepository _informeUtilizacionCargadorRepository;

        public InformeUtilizacionCargadorService(IInformeUtilizacionCargadorRepository informeUtilizacionCargadorRepository)
        {
            _informeUtilizacionCargadorRepository = informeUtilizacionCargadorRepository;
        }

        public async Task<IEnumerable<InformeUtilizacionCargador>> GetInformeUtilizacionAsync()
        {
            return await _informeUtilizacionCargadorRepository
                .GetInformeUtilizacionAsync();
        }

        public async Task<InformeUtilizacionCargador> GetInformeUtilizacionByIdAsync(int hora)
        {
            // Validamos que el informe operacion autobus exista
            var unInformeUtilizacionCargador = await _informeUtilizacionCargadorRepository
                .GetInformeUtilizacionByIdAsync(hora);

            if (unInformeUtilizacionCargador.Hora != hora)
                throw new AppValidationException($"La hora {hora} no existe en la base de datos");

            return unInformeUtilizacionCargador;
        }
    }
}

