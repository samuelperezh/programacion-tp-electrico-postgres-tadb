using ProgramacionTP_CS_API_PostgreSQL_Dapper.Models;
namespace ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces
{
    public interface IInformeRepository
    {
       public Task<Informe> GetAllAsync();

    }
}
