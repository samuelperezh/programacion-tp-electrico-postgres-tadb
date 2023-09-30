using ProgramacionTP_CS_API_PostgreSQL_Dapper.DbContexts;
using ProgramacionTB_CS_API_PostgreSQL_Dapper.Helpers;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces;
using ProgramacionTB_CS_API_PostgreSQL_Dapper.Models;
using Dapper;
using Npgsql;
using System.Data;

namespace ProgramacionTP_CS_API_PostgreSQL_Dapper.Repositories
    {
        public class OperacionAutobusRepository : IOperacionAutobusRepository
    {
            private readonly PgsqlDbContext contextoDB;

            public OperacionAutobusRepository(PgsqlDbContext unContexto)
            {
                contextoDB = unContexto;
            }

            public async Task<IEnumerable<OperacionAutobus>> GetAllAsync()
            {
                using (var conexion = contextoDB.CreateConnection())
                {
                    string sentenciaSQL = "SELECT autobus_id, horario_id"+
                                           "FROM operacion_autobuses" +
                                            "ORDER BY autobus_id DESC, horario_id DESC";

                    var resultadoOperacionAutobuses = await conexion.QueryAsync<OperacionAutobus>(sentenciaSQL,
                                            new DynamicParameters());

                    return resultadoOperacionAutobuses;
                }
            }

        public async Task<bool> CreateAsync(OperacionAutobus unaOperacionAutobus)
            {
                bool resultadoAccion = false;

                try
                {
                    using (var conexion = contextoDB.CreateConnection())
                    {
                        string procedimiento = "core.p_inserta_operacionAutobus";
                        var parametros = new
                        {
                            p_autobusId = unaOperacionAutobus.AutobusId,
                            p_horarioId = unaOperacionAutobus.HorarioId
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

            public async Task<bool> UpdateAsync(OperacionAutobus unaOperacionAutobus)
            {
                bool resultadoAccion = false;

                try
                {
                    using (var conexion = contextoDB.CreateConnection())
                    {
                        string procedimiento = "core.p_actualiza_operacionAutobus";
                        var parametros = new
                        {
                            p_autobusId = unaOperacionAutobus.AutobusId,
                            p_horarioId = unaOperacionAutobus.HorarioId

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

            public async Task<bool> DeleteAsync(OperacionAutobus unaOperacionAutobus)
            {
                bool resultadoAccion = false;

                try
                {
                    using (var conexion = contextoDB.CreateConnection())
                    {
                        string procedimiento = "core.p_elimina_unaOperacionAutobus";
                        var parametros = new
                        {
                            p_autobusId = unaOperacionAutobus.AutobusId,
                            p_horarioId = unaOperacionAutobus.HorarioId
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