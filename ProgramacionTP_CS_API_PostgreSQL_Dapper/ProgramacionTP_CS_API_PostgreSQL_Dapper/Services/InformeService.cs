using ProgramacionTB_CS_API_PostgreSQL_Dapper.Helpers;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Models;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Repositories;

namespace ProgramacionTP_CS_API_PostgreSQL_Dapper.Services
{
    public class InformeService
    {
        private readonly IInformeRepository _informeRepository;
        private readonly IHorarioRepository _horarioRepository;

        public InformeService(IInformeRepository informeRepository,
                              IHorarioRepository horarioRepository)
        {
            _informeRepository = informeRepository;
            _horarioRepository = horarioRepository;
        }

        public async Task<Informe> GetAllAsync()
        {
            return await _informeRepository
                .GetAllAsync();
        }

        public async Task<Informe> GetByIdAsync(int horario_id)
        {
            // Validamos que el horario exista con ese Id
            var unHorario = await _horarioRepository
                .GetByIdAsync(horario_id);

            if (unHorario.Id == 0)
                throw new AppValidationException($"Horario no encontrado con la hora {horario_id}");

            return await _informeRepository
                .GetByIdAsync(horario_id);
        }
    }
}

        
