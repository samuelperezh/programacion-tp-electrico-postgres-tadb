using ProgramacionTP_CS_API_PostgreSQL_Dapper.Models;
namespace ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces
{
    public interface IInformeRepository
    {
        public Task<Informe> GetAllAsync();
        public Task<Informe> GetByIdAsync(int horario_id);
    }
}
