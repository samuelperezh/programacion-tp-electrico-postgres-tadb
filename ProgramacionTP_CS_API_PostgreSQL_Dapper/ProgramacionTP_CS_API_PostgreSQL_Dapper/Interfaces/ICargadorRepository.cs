using ProgramacionTB_CS_API_PostgreSQL_Dapper.Models;

namespace ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces
{
    public interface ICargadorRepository
    {
        public Task<IEnumerable<Cargador>> GetAllAsync();
        public Task<Cargador> GetByIdAsync(int cargador_id);
        public Task<bool> CreateAsync(Cargador cargador);
        public Task<bool> UpdateAsync(Cargador cargador);
        public Task<bool> DeleteAsync(Cargador cargador);
    }
}