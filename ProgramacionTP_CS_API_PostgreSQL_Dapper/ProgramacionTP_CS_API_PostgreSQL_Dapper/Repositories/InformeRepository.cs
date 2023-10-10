﻿using Dapper;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.DbContexts;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Models;

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
    }
}
