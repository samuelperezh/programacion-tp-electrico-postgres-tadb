using ProgramacionTB_CS_API_PostgreSQL_Dapper.Helpers;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces;
using ProgramacionTB_CS_API_PostgreSQL_Dapper.Models;

namespace ProgramacionTB_CS_API_PostgreSQL_Dapper.Services
{
    public class HorarioService
    {
        private readonly IHorarioRepository _HorarioRepository;

        public HorarioService(IHorarioRepository HorarioRepository)
        {
            _HorarioRepository = HorarioRepository;
        }

        public async Task<IEnumerable<Horario>> GetAllAsync()
        {
            return await _HorarioRepository
                .GetAllAsync();
        }

        public async Task<Horario> GetByIdAsync(int Horario_id)
        {
            //Validamos que el Horario exista con ese Id
            var unHorario = await _HorarioRepository
                .GetByIdAsync(Horario_id);

            if (unHorario.Id == 0)
                throw new AppValidationException($"Horario no encontrado con el id {Horario_id}");

            return unHorario;
        }
    }
}

