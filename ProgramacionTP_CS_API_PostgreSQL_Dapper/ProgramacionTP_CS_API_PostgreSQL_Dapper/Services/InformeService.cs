using ProgramacionTB_CS_API_PostgreSQL_Dapper.Helpers;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Models;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Repositories;

namespace ProgramacionTP_CS_API_PostgreSQL_Dapper.Services
{
    public class InformeService
    {
        private readonly IInformeRepository _informeRepository;

        public InformeService(IInformeRepository informeRepository)
        {
            _informeRepository = informeRepository;
        }

        public async Task<Informe> GetInformeAsync()
        {
            return await _informeRepository
                .GetInformeAsync();
        }
    }
}

        
