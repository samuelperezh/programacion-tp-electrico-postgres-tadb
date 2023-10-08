using ProgramacionTB_CS_API_PostgreSQL_Dapper.Models;

namespace ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces
{
    public interface IAutobusRepository
    {
        public Task<IEnumerable<Autobus>> GetAllAsync();
        public Task<Autobus> GetByIdAsync(int autobus_id);
        public Task<Autobus> GetByNameAsync(string nombre_autobus);
        public Task<int> GetTotalAssociatedChargerUtilizationAsync(int cargador_id);
        public Task<int> GetTotalAssociatedAutobusOperationAsync(int cargador_id);
        public Task<bool> CreateAsync(Autobus unAutobus);
        public Task<bool> UpdateAsync(Autobus unAutobus);
        public Task<bool> DeleteAsync(Autobus unAutobus);
    }
}