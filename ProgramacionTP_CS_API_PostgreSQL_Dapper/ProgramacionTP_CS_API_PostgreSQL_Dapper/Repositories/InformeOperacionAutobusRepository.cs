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
        public async Task<IEnumerable<InformeOperacionAutobus>> GetInformeOperacionAsync()
        {
            using (var conexion = contextoDB.CreateConnection())
            {
                string sentenciaSQL = "SELECT * FROM v_total_operacion_autobuses";

                var unInformeOperacionAutobus = await conexion.QueryAsync<InformeOperacionAutobus>(sentenciaSQL,
                                        new DynamicParameters());

                return unInformeOperacionAutobus;
            }
        }
    }
}
