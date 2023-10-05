using ProgramacionTB_CS_API_PostgreSQL_Dapper.Helpers;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces;
using ProgramacionTB_CS_API_PostgreSQL_Dapper.Models;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Repositories;

namespace ProgramacionTB_CS_API_PostgreSQL_Dapper.Services
{
    public class OperacionAutobusService
    {
        private readonly IOperacionAutobusRepository _OperacionAutobusRepository;

        public OperacionAutobusService(IOperacionAutobusRepository OperacionAutobusRepository)
        {
            _OperacionAutobusRepository = OperacionAutobusRepository;
        }

        public async Task<IEnumerable<OperacionAutobus>> GetAllAsync()
        {
            return await _OperacionAutobusRepository
                .GetAllAsync();
        }
        public async Task<IEnumerable<OperacionAutobus>> GetAssociatedOperationsAsync(int autobus_id, int horario_id)
        {
            // Validar que el autobús esté en operación en horario pico
            if (!IsAutobusInPicoHorario(horario_id))
            {
                throw new AppValidationException($"El autobús no está en operación en horario pico.");
            }

            // Obtener las operaciones de autobuses asociadas
            var operaciones = await _OperacionAutobusRepository.GetAssociatedOperationsAsync(autobus_id, horario_id);

            return operaciones;
        }
        private bool IsAutobusInPicoHorario(int horario_id)
        {
            // Validar si el horario está en horario pico (ID de horario entre 5-9 y 16-20)
            return (horario_id >= 5 && horario_id <= 9) || (horario_id >= 16 && horario_id <= 20);
        }


    }
}