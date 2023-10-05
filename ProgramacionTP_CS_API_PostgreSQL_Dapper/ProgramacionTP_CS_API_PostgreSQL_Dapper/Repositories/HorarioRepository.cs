using ProgramacionTP_CS_API_PostgreSQL_Dapper.DbContexts;
using ProgramacionTB_CS_API_PostgreSQL_Dapper.Helpers;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces;
using ProgramacionTB_CS_API_PostgreSQL_Dapper.Models;
using Dapper;
using Npgsql;
using System.Data;

namespace ProgramacionTP_CS_API_PostgreSQL_Dapper.Repositories
{
    public class HorarioRepository : IHorarioRepository
    {
        private readonly PgsqlDbContext contextoDB;

        public HorarioRepository(PgsqlDbContext unContexto)
        {
            contextoDB = unContexto;
        }

        public async Task<IEnumerable<Horario>> GetAllAsync()
        {
            using (var conexion = contextoDB.CreateConnection())
            {
                string sentenciaSQL = "SELECT h.id, h.horario_pico FROM horarios h ORDER BY h.id DESC";

                var resultadoHorarios = await conexion.QueryAsync<Horario>(sentenciaSQL,
                                        new DynamicParameters());

                return resultadoHorarios;
            }
        }

        public async Task<Horario> GetByIdAsync(int horario_id)
        {
            Horario unHorario = new Horario();

            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@horario_id", horario_id,
                                        DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT h.id, h.horario_pico FROM horarios h WHERE h.id = @horario_id ";

                var resultado = await conexion.QueryAsync<Horario>(sentenciaSQL,
                                    parametrosSentencia);

                if (resultado.Count() > 0)
                    unHorario = resultado.First();
            }

            return unHorario;
        }
        public async Task<Horario> GetHorariosPicoAsync() 
        {
            Horario unhorarioPico = new Horario();

            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@horarioPico", true, DbType.Boolean, ParameterDirection.Input);

                string sentenciaSQL = "SELECT id, horario_pico FROM horarios WHERE horario_pico = @horarioPico";


                var resultado = await conexion.QueryAsync<Horario>(sentenciaSQL, parametrosSentencia);

                if (resultado.Count() > 0)
                    unhorarioPico = resultado.First();

            }
            return unhorarioPico;
        }
    }
}