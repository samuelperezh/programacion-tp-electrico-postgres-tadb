using ProgramacionTB_CS_API_PostgreSQL_Dapper.Helpers;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces;
using ProgramacionTB_CS_API_PostgreSQL_Dapper.Models;

namespace ProgramacionTB_CS_API_PostgreSQL_Dapper.Services
{
    public class HorarioService
    {
        private readonly IHorarioRepository _horarioRepository;

        public HorarioService(IHorarioRepository horarioRepository)
        {
            _horarioRepository = horarioRepository;
        }

        public async Task<IEnumerable<Horario>> GetAllAsync()
        {
            return await _horarioRepository
                .GetAllAsync();
        }

        public async Task<Horario> GetByIdAsync(int horario_id)
        {
            //Validamos que el horario exista con ese Id
            var unHorario = await _horarioRepository
                .GetByIdAsync(horario_id);

            if (unHorario.Id == 0)
                throw new AppValidationException($"Horario no encontrado con el id {horario_id}");

            return unHorario;
        }
    }
}