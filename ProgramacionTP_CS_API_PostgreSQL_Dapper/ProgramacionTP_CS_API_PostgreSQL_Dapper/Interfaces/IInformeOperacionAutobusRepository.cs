using ProgramacionTP_CS_API_PostgreSQL_Dapper.Models;

namespace ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces
{
    public interface IInformeOperacionAutobusRepository
    {
        public Task<IEnumerable<InformeOperacionAutobus>> GetInformeOperacionAsync();
    }
}
