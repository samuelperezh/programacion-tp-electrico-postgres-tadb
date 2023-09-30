﻿using ProgramacionTP_CS_API_PostgreSQL_Dapper.DbContexts;
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
                    string sentenciaSQL = "SELECT c.id, c.nombre, c.sitio_web, c.instagram, " +
                                          "(u.municipio || ', ' || u.departamento) ubicacion, c.ubicacion_id " +
                                          "FROM cervecerias c JOIN ubicaciones u " +
                                          "ON c.ubicacion_id = u.id " +
                                          "ORDER BY c.id DESC";

                    var resultadoOperacionAutobuses = await conexion.QueryAsync<OperacionAutobus>(sentenciaSQL,
                                            new DynamicParameters());

                    return resultadoOperacionAutobuses;
                }
            }

        public async Task<OperacionAutobus> GetByIdAsync(int autobus_id)
        {
            OperacionAutobus unaOperacionAutobus = new OperacionAutobus();

            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@autobus_id", autobus_id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT c.id, c.nombre, c.sitio_web, c.instagram, " +
                                      "(u.municipio || ', ' || u.departamento) ubicacion, c.ubicacion_id " +
                                      "FROM cervecerias c JOIN ubicaciones u ON c.ubicacion_id = u.id " +
                                      "WHERE c.id = @cerveceria_id ";

                var resultado = await conexion.QueryAsync<OperacionAutobus>(sentenciaSQL,
                                    parametrosSentencia);

                if (resultado.Count() > 0)
                    unaOperacionAutobus = resultado.First();
            }

            return unaOperacionAutobus;
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