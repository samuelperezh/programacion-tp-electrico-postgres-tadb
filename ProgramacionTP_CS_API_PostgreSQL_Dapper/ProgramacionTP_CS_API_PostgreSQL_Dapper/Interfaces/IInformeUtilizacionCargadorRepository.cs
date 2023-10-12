using ProgramacionTP_CS_API_PostgreSQL_Dapper.Models;

namespace ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces
{
    public interface IInformeUtilizacionCargadorRepository
    {
        public Task<IEnumerable<InformeUtilizacionCargador>> GetInformeUtilizacionAsync();
    }
}
