using ProgramacionTB_CS_API_PostgreSQL_Dapper.Helpers;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Models;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Repositories;

namespace ProgramacionTP_CS_API_PostgreSQL_Dapper.Services
{
    public class InformeOperacionAutobusService
    {
        private readonly IInformeOperacionAutobusRepository _informeOperacionAutobusRepository;

        public InformeOperacionAutobusService(IInformeOperacionAutobusRepository informeOperacionAutobusRepository)
        {
            _informeOperacionAutobusRepository = informeOperacionAutobusRepository;
        }

        public async Task<InformeOperacionAutobus> GetInformeOperacionAsync()
        {
            return await _informeOperacionAutobusRepository
                .GetInformeOperacionAsync();
        }
    }
}

