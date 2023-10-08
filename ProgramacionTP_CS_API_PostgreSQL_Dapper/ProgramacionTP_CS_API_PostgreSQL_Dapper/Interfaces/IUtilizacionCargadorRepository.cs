using ProgramacionTB_CS_API_PostgreSQL_Dapper.Models;

namespace ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces
{
    public interface IUtilizacionCargadorRepository
    {
        public Task<IEnumerable<UtilizacionCargador>> GetAllAsync();
        public Task<UtilizacionCargador> GetByUtilizationAsync(int cargador_id, int autobus_id, int horario_id);
        public Task<bool> CreateAsync(UtilizacionCargador unaUtilizacionCargador);
        public Task<bool> UpdateAsync(UtilizacionCargador unaUtilizacionCargador);
        public Task<bool> DeleteAsync(UtilizacionCargador unaUtilizacionCargador);
    }
}
