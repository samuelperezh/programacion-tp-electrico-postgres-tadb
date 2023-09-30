using ProgramacionTB_CS_API_PostgreSQL_Dapper.Models;

namespace ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces
{
    public interface IAutobusRepository
    {
        public Task<IEnumerable<Autobus>> GetAllAsync();
        public Task<Autobus> GetByIdAsync(int autobus_id);

    }
}