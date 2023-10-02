using ProgramacionTB_CS_API_PostgreSQL_Dapper.Models;

namespace ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces
{
    public interface ICargadorRepository
    {
        public Task<IEnumerable<Cargador>> GetAllAsync();
        public Task<Cargador> GetByIdAsync(int cargador_id);
        public Task<Cargador> GetByNameAsync(string cargador_nombre);
        public Task<int> GetTotalAssociatedChargerUtilizationAsync(int cargador_id);
        public Task<bool> CreateAsync(Cargador unCargador);
        public Task<bool> UpdateAsync(Cargador unCargador);
        public Task<bool> DeleteAsync(Cargador unCargador);
    }
}