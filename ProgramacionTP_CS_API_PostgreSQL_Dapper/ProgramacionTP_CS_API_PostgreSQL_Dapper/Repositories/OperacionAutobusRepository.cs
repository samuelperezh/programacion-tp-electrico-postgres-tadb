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
                string sentenciaSQL = "SELECT autobus_id, horario_id " +
                                      "FROM operacion_autobuses " +
                                      "ORDER BY autobus_id, horario_id";

                var resultadoOperacionAutobuses = await conexion.QueryAsync<OperacionAutobus>(sentenciaSQL,
                                        new DynamicParameters());

                return resultadoOperacionAutobuses;
            }
        }
        public async Task<OperacionAutobus> GetByOperationAsync(int autobus_id, int horario_id)
        {
            OperacionAutobus unaOperacionAutobus = new OperacionAutobus();

            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@autobus_id", autobus_id,
                                                           DbType.Int32, ParameterDirection.Input);
                parametrosSentencia.Add("@horario_id", horario_id,
                                                           DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT autobus_id, horario_id " +
                                      "FROM operacion_autobuses " +
                                      "WHERE autobus_id = @autobus_id AND horario_id = @horario_id";

                var resultado = await conexion.QueryAsync<OperacionAutobus>(sentenciaSQL,
                                                       parametrosSentencia);

                if (resultado.Count() > 0)
                    unaOperacionAutobus = resultado.First();
            }

            return unaOperacionAutobus;
        }

        public async Task<string> GetAutobusStateAsync(int horario_id, int autobus_id)
        {
            string estado;

            using (var connection = contextoDB.CreateConnection())
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@horario_id", horario_id, DbType.Int32, ParameterDirection.Input);
                parameters.Add("@autobus_id", autobus_id, DbType.Int32, ParameterDirection.Input);

                string sqlQuery = "SELECT * FROM f_estado_autobus(@horario_id, @autobus_id)";

                estado = await connection.QueryFirstOrDefaultAsync<string>(sqlQuery, parameters);
            }

            return estado;
        }
        public async Task<bool> CreateAsync(OperacionAutobus unaOperacionAutobus)
        {
            bool resultadoAccion = false;

            try
            {
                using (var conexion = contextoDB.CreateConnection())
                {
                    string procedimiento = "p_inserta_operacion_autobus";
                    var parametros = new
                    {
                        p_autobus_id = unaOperacionAutobus.Autobus_id,
                        p_horario_id = unaOperacionAutobus.Horario_id
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
                    string procedimiento = "p_actualiza_operacion_autobus";
                    var parametros = new
                    {
                        p_autobus_id = unaOperacionAutobus.Autobus_id,
                        p_horario_id = unaOperacionAutobus.Horario_id
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
                    string procedimiento = "p_elimina_operacion_autobus";
                    var parametros = new
                    {
                        p_autobus_id = unaOperacionAutobus.Autobus_id,
                        p_horario_id = unaOperacionAutobus.Horario_id
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