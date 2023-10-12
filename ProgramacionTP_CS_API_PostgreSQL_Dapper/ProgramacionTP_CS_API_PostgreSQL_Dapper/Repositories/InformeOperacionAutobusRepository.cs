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

        public async Task<InformeOperacionAutobus> GetInformeOperacionByIdAsync(int hora)
        {
            InformeOperacionAutobus unInformeOperacionAutobus = new InformeOperacionAutobus();

            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@hora", hora,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT * FROM v_total_operacion_autobuses WHERE hora = @hora";

                var resultado = await conexion.QueryAsync<InformeOperacionAutobus>(sentenciaSQL,
                                    parametrosSentencia);

                if (resultado.Count() > 0)
                    unInformeOperacionAutobus = resultado.First();
            }

            return unInformeOperacionAutobus;
        }
    }
}
