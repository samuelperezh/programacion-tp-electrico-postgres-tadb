using ProgramacionTP_CS_API_PostgreSQL_Dapper.DbContexts;
using ProgramacionTB_CS_API_PostgreSQL_Dapper.Helpers;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces;
using ProgramacionTB_CS_API_PostgreSQL_Dapper.Models;
using Dapper;
using Npgsql;
using System.Data;

namespace ProgramacionTP_CS_API_PostgreSQL_Dapper.Repositories
{
    public class AutobusRepository : IAutobusRepository
    {
        private readonly PgsqlDbContext contextoDB;

        public AutobusRepository(PgsqlDbContext unContexto)
        {
            contextoDB = unContexto;
        }

        public async Task<IEnumerable<Autobus>> GetAllAsync()
        {
            using (var conexion = contextoDB.CreateConnection())
            {
                string sentenciaSQL = "SELECT a.id, a.autobus FROM autobuses a ORDER BY a.id DESC";

                var resultadoAutobuses = await conexion.QueryAsync<Autobus>(sentenciaSQL,
                                        new DynamicParameters());

                return resultadoAutobuses;
            }
        }

        public async Task<Autobus> GetByIdAsync(int autobus_id)
        {
            Autobus unAutobus = new Autobus();

            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@autobus_id", autobus_id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT a.id, a.autobus FROM autobuses a WHERE a.id = @autobus_id";

                var resultado = await conexion.QueryAsync<Autobus>(sentenciaSQL,
                                    parametrosSentencia);

                if (resultado.Count() > 0)
                    unAutobus = resultado.First();
            }

            return unAutobus;
        }

        public async Task<Autobus> GetByNameAsync(string autobus_nombre)
        {
            Autobus unAutobus = new Autobus();

            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@autobus_nombre", autobus_nombre,
                                        DbType.String, ParameterDirection.Input);

                string sentenciaSQL = "SELECT id, autobus FROM autobuses WHERE LOWER(autobus) = LOWER(@autobus_nombre)";

                var resultado = await conexion.QueryAsync<Autobus>(sentenciaSQL,
                                    parametrosSentencia);

                if (resultado.Count() > 0)
                    unAutobus = resultado.First();
            }

            return unAutobus;
        }

        public async Task<int> GetTotalAssociatedChargerUtilizationAsync(int autobus_id)
        {
            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@autobus_id", autobus_id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT COUNT(id) totalUtilizaciones " +
                                      "FROM utilizacion_cargadores " +
                                      "WHERE autobus_id = @autobus_id ";

                var totalUtilizaciones = await conexion.QueryFirstAsync<int>(sentenciaSQL,
                                        parametrosSentencia);

                return totalUtilizaciones;
            }
        }

        public async Task<int> GetTotalAssociatedAutobusOperationAsync(int autobus_id)
        {
            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@autobus_id", autobus_id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT COUNT(id) totalOperaciones " +
                                      "FROM operacion_autobuses " +
                                      "WHERE autobus_id = @autobus_id ";

                var totalOperaciones = await conexion.QueryFirstAsync<int>(sentenciaSQL,
                                        parametrosSentencia);

                return totalOperaciones;
            }
        }

        public async Task<bool> CreateAsync(Autobus unAutobus)
        {
            bool resultadoAccion = false;

            try
            {
                using (var conexion = contextoDB.CreateConnection())
                {
                    string procedimiento = "core.p_inserta_autobus";
                    var parametros = new
                    {
                        p_nombre = unAutobus.Nombre
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

        public async Task<bool> UpdateAsync(Autobus unAutobus)
        {
            bool resultadoAccion = false;

            try
            {
                using (var conexion = contextoDB.CreateConnection())
                {
                    string procedimiento = "core.p_actualiza_autobus";
                    var parametros = new
                    {
                        p_id = unAutobus.Id,
                        p_nombre = unAutobus.Nombre

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

        public async Task<bool> DeleteAsync(Autobus unAutobus)
        {
            bool resultadoAccion = false;

            try
            {
                using (var conexion = contextoDB.CreateConnection())
                {
                    string procedimiento = "core.p_elimina_autobus";
                    var parametros = new
                    {
                        p_id = unAutobus.Id,
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