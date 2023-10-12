using ProgramacionTP_CS_API_PostgreSQL_Dapper.DbContexts;
using ProgramacionTB_CS_API_PostgreSQL_Dapper.Helpers;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Models;
using Dapper;
using Npgsql;
using System.Data;

namespace ProgramacionTP_CS_API_PostgreSQL_Dapper.Repositories
{
    public class InformeOperacionAutobusRepository : IInformeOperacionAutobusRepository
    {
        private readonly PgsqlDbContext contextoDB;

        public InformeOperacionAutobusRepository(PgsqlDbContext unContexto)
        {
            contextoDB = unContexto;
        }
        public async Task<InformeOperacionAutobus> GetInformeOperacionAsync()
        {
            InformeOperacionAutobus unInformeOperacionAutobus = new InformeOperacionAutobus();

            using (var conexion = contextoDB.CreateConnection())
            {
                //Total Horarios y operacion autobuses
                string sentenciaSQL = "SELECT * FROM v_total_operacion_autobuses";
                unInformeOperacionAutobus.Hora = await conexion.QueryFirstAsync<int>(sentenciaSQL, new DynamicParameters());
            }
            return unInformeOperacionAutobus;
        }
    }
}
