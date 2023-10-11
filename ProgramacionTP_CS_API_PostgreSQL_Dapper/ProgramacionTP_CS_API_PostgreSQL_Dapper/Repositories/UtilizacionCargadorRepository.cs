using ProgramacionTP_CS_API_PostgreSQL_Dapper.DbContexts;
using ProgramacionTB_CS_API_PostgreSQL_Dapper.Helpers;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces;
using ProgramacionTB_CS_API_PostgreSQL_Dapper.Models;
using Dapper;
using Npgsql;
using System.Data;

namespace ProgramacionTP_CS_API_PostgreSQL_Dapper.Repositories
{
    public class UtilizacionCargadorRepository : IUtilizacionCargadorRepository
    {
        private readonly PgsqlDbContext contextoDB;

        public UtilizacionCargadorRepository(PgsqlDbContext unContexto)
        {
            contextoDB = unContexto;
        }

        public async Task<IEnumerable<UtilizacionCargador>> GetAllAsync()
        {
            using (var conexion = contextoDB.CreateConnection())
            {
                string sentenciaSQL = "SELECT u.cargador_id, u.autobus_id, horario_id " +
                                      "FROM utilizacion_cargadores u " +
                                      "ORDER BY u.cargador_id";

                var resultadoUtilizacionCargadores = await conexion.QueryAsync<UtilizacionCargador>(sentenciaSQL,
                                        new DynamicParameters());

                return resultadoUtilizacionCargadores;
            }
        }

        public async Task<UtilizacionCargador> GetByUtilizationAsync(int cargador_id, int autobus_id, int horario_id)
        {
            UtilizacionCargador unaUtilizacionCargador = new UtilizacionCargador();

            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@cargador_id", cargador_id,
                                        DbType.Int32, ParameterDirection.Input);
                parametrosSentencia.Add("@autobus_id", autobus_id,
                                        DbType.Int32, ParameterDirection.Input);
                parametrosSentencia.Add("@horario_id", horario_id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT cargador_id, autobus_id, horario_id " +
                                      "FROM utilizacion_cargadores " +
                                      "WHERE cargador_id = @cargador_id AND autobus_id= @autobus_id AND horario_id = @horario_id";

                var resultado = await conexion.QueryAsync<UtilizacionCargador>(sentenciaSQL,
                                    parametrosSentencia);

                if (resultado.Count() > 0)
                    unaUtilizacionCargador = resultado.First();
            }

            return unaUtilizacionCargador;
        }

        public async Task<bool> CreateAsync(UtilizacionCargador unaUtilizacionCargador)
        {
            bool resultadoAccion = false;

            try
            {
                using (var conexion = contextoDB.CreateConnection())
                {
                    string procedimiento = "p_inserta_utilizacion_cargador";
                    var parametros = new
                    {
                        p_cargador_id = unaUtilizacionCargador.Cargador_id,
                        p_autobus_id = unaUtilizacionCargador.Autobus_id,
                        p_horario_id = unaUtilizacionCargador.Horario_id
                    };

                    var cantidad_filas = await conexion.ExecuteAsync(
                        procedimiento,
                        parametros,
                        commandType: CommandType.StoredProcedure);

                    if (cantidad_filas != 0)
                        resultadoAccion = true;
                }
            }
            catch (NpgsqlException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }

        public async Task<bool> UpdateAsync(UtilizacionCargador unaUtilizacionCargador)
        {
            bool resultadoAccion = false;

            try
            {
                using (var conexion = contextoDB.CreateConnection())
                {
                    string procedimiento = "p_actualiza_utilizacion_cargador";
                    var parametros = new
                    {
                        p_cargador_id = unaUtilizacionCargador.Cargador_id,
                        p_autobus_id = unaUtilizacionCargador.Autobus_id,
                        p_horario_id = unaUtilizacionCargador.Horario_id

                    };

                    var cantidad_filas = await conexion.ExecuteAsync(
                        procedimiento,
                        parametros,
                        commandType: CommandType.StoredProcedure);

                    if (cantidad_filas != 0)
                        resultadoAccion = true;
                }
            }
            catch (NpgsqlException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }

        public async Task<bool> DeleteAsync(UtilizacionCargador unaUtilizacionCargador)
        {
            bool resultadoAccion = false;

            try
            {
                using (var conexion = contextoDB.CreateConnection())
                {
                    string procedimiento = "p_elimina_utilizacion_cargador";
                    var parametros = new
                    {
                        p_cargador_id = unaUtilizacionCargador.Cargador_id,
                        p_autobus_id = unaUtilizacionCargador.Autobus_id,
                        p_horario_id = unaUtilizacionCargador.Horario_id
                    };

                    var cantidad_filas = await conexion.ExecuteAsync(
                        procedimiento,
                        parametros,
                        commandType: CommandType.StoredProcedure);

                    if (cantidad_filas != 0)
                        resultadoAccion = true;
                }
            }
            catch (NpgsqlException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }
    }
}