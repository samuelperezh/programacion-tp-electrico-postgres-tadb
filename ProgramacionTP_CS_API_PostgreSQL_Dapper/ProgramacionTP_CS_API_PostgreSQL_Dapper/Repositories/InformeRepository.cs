using ProgramacionTP_CS_API_PostgreSQL_Dapper.DbContexts;
using ProgramacionTB_CS_API_PostgreSQL_Dapper.Helpers;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Models;
using Dapper;
using Npgsql;
using System.Data;

namespace ProgramacionTP_CS_API_PostgreSQL_Dapper.Repositories
{
    public class InformeRepository : IInformeRepository
    {
        private readonly PgsqlDbContext contextoDB;

        public InformeRepository(PgsqlDbContext unContexto)
        {
            contextoDB = unContexto;
        }

        public async Task<Informe> GetAllAsync()
        {
            Informe unInforme = new Informe();

            using (var conexion = contextoDB.CreateConnection())
            {
                //Total Horarios
                string sentenciaSQL = "SELECT COUNT(id) total FROM horarios";
                unInforme.Horarios = await conexion.QueryFirstAsync<int>(sentenciaSQL, new DynamicParameters());

                //Total cargadores
                sentenciaSQL = "SELECT COUNT(id) total FROM cargadores";
                unInforme.Cargadores = await conexion.QueryFirstAsync<int>(sentenciaSQL, new DynamicParameters());

                //Total autobuses
                sentenciaSQL = "SELECT COUNT(id) total FROM autobuses";
                unInforme.Autobuses = await conexion.QueryFirstAsync<int>(sentenciaSQL, new DynamicParameters());

                //Total operacion autobuses
                sentenciaSQL = "SELECT COUNT(autobus_id) total FROM operacion_autobuses";
                unInforme.OperacionAutobuses = await conexion.QueryFirstAsync<int>(sentenciaSQL, new DynamicParameters());

                //Total utilización cargadores
                sentenciaSQL = "SELECT COUNT(cargador_id) total FROM utilizacion_cargadores";
                unInforme.UtilizacionCargadores = await conexion.QueryFirstAsync<int>(sentenciaSQL, new DynamicParameters());
            }
            return unInforme;
        }

        public async Task<Informe> GetByIdAsync(int horario_id)
        {
            Informe unInforme = new Informe();

            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@horario_id", horario_id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT " +
                                      "(select f_porcentaje_autobuses_operacion(h.id) OperacionAutobuses " +
                                      "from horarios h where h.id=@horario_id) porcentaje_autobuses_operacion, " +
                                      "(select f_porcentaje_cargadores_utilizados(h.id) UtilizacionCargadores " +
                                      "from horarios h where h.id=@horario_id) porcentaje_cargadores_utilizados;";

                var resultado = await conexion.QueryAsync<Informe>(sentenciaSQL,
                                    parametrosSentencia);

                if (resultado.Count() > 0)
                    unInforme = resultado.First();
            }

            return unInforme;
        }
    }
}
