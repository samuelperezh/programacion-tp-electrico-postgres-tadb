using ProgramacionTB_CS_API_PostgreSQL_Dapper.Models;

namespace ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces
{
    public interface IOperacionAutobusRepository
    {

        public Task<IEnumerable<OperacionAutobus>> GetAllAsync();
        public Task<bool> CreateAsync(OperacionAutobus unaOperacionAutobus);
        public Task<bool> UpdateAsync(OperacionAutobus unaOperacionAutobus);
        public Task<bool> DeleteAsync(OperacionAutobus unaOperacionAutobus);
    }
}
