using ProgramacionTB_CS_API_PostgreSQL_Dapper.Helpers;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Models;

namespace ProgramacionTB_CS_API_PostgreSQL_Dapper.Services
{
    public class InformeHoraService
    {
        private readonly IInformeHoraRepository _informeHoraRepository;

        public InformeHoraService(IInformeHoraRepository informeHoraRepository)
        {
            _informeHoraRepository = informeHoraRepository;
        }

        public async Task<InformeHora> GetInformeHoraAsync(int hora)
        {
            // Validamos que el informeHora exista con ese Id
            var unInformeHora = await _informeHoraRepository
                .GetInformeHoraAsync(hora);

            if (unInformeHora.Hora < 0 || unInformeHora.Hora > 23)
                throw new AppValidationException($"La hora no fue encontrada con el id {hora}");

            return unInformeHora;
        }
    }
}