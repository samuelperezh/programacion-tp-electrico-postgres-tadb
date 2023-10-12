using ProgramacionTP_CS_API_PostgreSQL_Dapper.DbContexts;
using ProgramacionTB_CS_API_PostgreSQL_Dapper.Helpers;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Models;
using Dapper;
using Npgsql;
using System.Data;

namespace ProgramacionTP_CS_API_PostgreSQL_Dapper.Repositories
{
    public class InformeHoraRepository : IInformeHoraRepository
    {
        private readonly PgsqlDbContext contextoDB;

        public InformeHoraRepository(PgsqlDbContext unContexto)
        {
            contextoDB = unContexto;
        }

        public async Task<InformeHora> GetInformeHoraAsync(int hora)
        {
            InformeHora unInforme = new InformeHora();

            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@hora", hora, DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "select hora from v_porcentajes where hora = @hora";
                unInforme.Hora = await conexion.QueryFirstAsync<int>(sentenciaSQL, parametrosSentencia);

                sentenciaSQL = "select horario_pico from v_porcentajes where hora = @hora";
                unInforme.Horario_pico = await conexion.QueryFirstAsync<bool>(sentenciaSQL, parametrosSentencia);

                sentenciaSQL = "select porcentaje_cargadores_utilizados from v_porcentajes where hora = @hora";
                unInforme.Porcentaje_cargadores_utilizados = await conexion.QueryFirstAsync<float>(sentenciaSQL, parametrosSentencia);

                sentenciaSQL = "select porcentaje_autobuses_operacion from v_porcentajes where hora = @hora";
                unInforme.Porcentaje_autobuses_operacion = await conexion.QueryFirstAsync<float>(sentenciaSQL, parametrosSentencia);
            }

            return unInforme;
        }
    }
}
