using ProgramacionTP_CS_API_PostgreSQL_Dapper.Models;

namespace ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces
{
    public interface IInformeHoraRepository
    {
        public Task<IEnumerable<InformeHora>> GetAllInformeHoraAsync();
        public Task<InformeHora> GetInformeHoraAsync(int hora);
    }
}