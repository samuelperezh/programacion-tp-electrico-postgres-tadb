using ProgramacionTB_CS_API_PostgreSQL_Dapper.Models;

namespace ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces
{
    public interface IHorarioRepository
    {
        public Task<IEnumerable<Horario>> GetAllAsync();
        public Task<Horario> GetByIdAsync(int horario_id);
        public Task<Horario> GetHorariosPicoAsync();
    }
}