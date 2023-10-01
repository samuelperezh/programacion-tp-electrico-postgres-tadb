using ProgramacionTP_CS_API_PostgreSQL_Dapper.DbContexts;
using ProgramacionTB_CS_API_PostgreSQL_Dapper.Helpers;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces;
using ProgramacionTB_CS_API_PostgreSQL_Dapper.Models;
using Dapper;
using Npgsql;
using System.Data;

namespace ProgramacionTP_CS_API_PostgreSQL_Dapper.Repositories
{
    public class CargadorRepository : ICargadorRepository
    {
        private readonly PgsqlDbContext contextoDB;

        public CargadorRepository(PgsqlDbContext unContexto)
        {
            contextoDB = unContexto;
        }

        public async Task<IEnumerable<Cargador>> GetAllAsync()
        {
            using (var conexion = contextoDB.CreateConnection())
            {
                string sentenciaSQL = "SELECT c.id, c.cargador FROM cargadores c ORDER BY c.id DESC";

                var resultadoCargadores = await conexion.QueryAsync<Cargador>(sentenciaSQL,
                                        new DynamicParameters());

                return resultadoCargadores;
            }
        }

        public async Task<Cargador> GetByIdAsync(int cargador_id)
        {
            Cargador unCargador = new Cargador();

            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@cargador_id", cargador_id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT c.id, c.cargador FROM cargadores c WHERE c.id = @cargador_id";

                var resultado = await conexion.QueryAsync<Cargador>(sentenciaSQL,
                                    parametrosSentencia);

                if (resultado.Count() > 0)
                    unCargador = resultado.First();
            }

            return unCargador;
        }

        public async Task<Cargador> GetByNameAsync(string cargador_nombre)
        {
            Cargador unCargador = new Cargador();

            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@cargador_nombre", cargador_nombre,
                                        DbType.String, ParameterDirection.Input);

                string sentenciaSQL = "SELECT c.id, c.cargador FROM cargadores c WHERE LOWER(c.cargador) = LOWER(@cargador_nombre)";

                var resultado = await conexion.QueryAsync<Cargador>(sentenciaSQL,
                                    parametrosSentencia);

                if (resultado.Count() > 0)
                    unCargador = resultado.First();
            }

            return unCargador;
        }

        public async Task<int> GetTotalAssociatedChargersAsync(int cargador_id)
        {
            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@cargador_id", cargador_id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT COUNT(id) totalCargadores " +
                                      "FROM cargadores " +
                                      "WHERE cargador_id = @cargador_id ";

                var totalCargadores = await conexion.QueryFirstAsync<int>(sentenciaSQL,
                                        parametrosSentencia);

                return totalCargadores;
            }
        }

        public async Task<bool> CreateAsync(Cargador unCargador)
        {
            bool resultadoAccion = false;

            try
            {
                using (var conexion = contextoDB.CreateConnection())
                {
                    string procedimiento = "core.p_inserta_cargador";
                    var parametros = new
                    {
                        p_nombre = unCargador.Nombre
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

        public async Task<bool> UpdateAsync(Cargador unCargador)
        {
            bool resultadoAccion = false;

            try
            {
                using (var conexion = contextoDB.CreateConnection())
                {
                    string procedimiento = "core.p_actualiza_cargador";
                    var parametros = new
                    {
                        p_id = unCargador.Id,
                        p_nombre = unCargador.Nombre

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

        public async Task<bool> DeleteAsync(Cargador unCargador)
        {
            bool resultadoAccion = false;

            try
            {
                using (var conexion = contextoDB.CreateConnection())
                {
                    string procedimiento = "core.p_elimina_cargador";
                    var parametros = new
                    {
                        p_id = unCargador.Id,
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