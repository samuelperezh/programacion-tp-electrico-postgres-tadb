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
                string sentenciaSQL = "SELECT c.id, c.nombre, c.sitio_web, c.instagram, " +
                                      "(u.municipio || ', ' || u.departamento) ubicacion, c.ubicacion_id " +
                                      "FROM cervecerias c JOIN ubicaciones u " +
                                      "ON c.ubicacion_id = u.id " +
                                      "ORDER BY c.id DESC";

                var resultadoUtilizacionCargadores = await conexion.QueryAsync<UtilizacionCargador>(sentenciaSQL,
                                        new DynamicParameters());

                return resultadoUtilizacionCargadores;
            }
        }

        public async Task<bool> CreateAsync(UtilizacionCargador unaUtilizacionCargador)
        {
            bool resultadoAccion = false;

            try
            {
                using (var conexion = contextoDB.CreateConnection())
                {
                    string procedimiento = "core.p_inserta_utilizacionCargador";
                    var parametros = new
                    {
                        p_cargadorId = unaUtilizacionCargador.CargadorId,
                        p_autobusId = unaUtilizacionCargador.AutobusId,
                        p_horarioId = unaUtilizacionCargador.HorarioId
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
                    string procedimiento = "core.p_actualiza_utilizacionCargador";
                    var parametros = new
                    {
                        p_cargadorId = unaUtilizacionCargador.CargadorId,
                        p_autobusId = unaUtilizacionCargador.AutobusId,
                        p_horarioId = unaUtilizacionCargador.HorarioId

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
                    string procedimiento = "core.p_elimina_utilizacionCargador";
                    var parametros = new
                    {
                        p_cargadorId = unaUtilizacionCargador.CargadorId,
                        p_autobusId = unaUtilizacionCargador.AutobusId,
                        p_horarioId = unaUtilizacionCargador.HorarioId
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