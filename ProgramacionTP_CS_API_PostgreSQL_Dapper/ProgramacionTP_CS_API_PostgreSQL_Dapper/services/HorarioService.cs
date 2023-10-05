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
        public async Task<Horario> GetHorarioPicoByIdAsync(int horario_id)
        {
            // Verificar si el horario_id está en los rangos especificados para horario pico
            if ((horario_id >= 5 && horario_id <= 9) || (horario_id >= 16 && horario_id <= 20))
            {
                // Obtener el horario correspondiente
                var horario = await GetByIdAsync(horario_id);
                return horario;
            }
            else
            {
                throw new AppValidationException($"El horario con el id {horario_id} no es un horario pico.");
            }
        }
    }
}