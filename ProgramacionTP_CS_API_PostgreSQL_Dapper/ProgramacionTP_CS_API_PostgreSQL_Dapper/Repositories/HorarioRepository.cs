using ProgramacionTP_CS_API_PostgreSQL_Dapper.DbContexts;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Helpers;
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
                string sentenciaSQL = "SELECT c.id, c.nombre, c.sitio_web, c.instagram, " +
                                      "(u.municipio || ', ' || u.departamento) ubicacion, c.ubicacion_id " +
                                      "FROM cervecerias c JOIN ubicaciones u " +
                                      "ON c.ubicacion_id = u.id " +
                                      "ORDER BY c.id DESC";

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

                string sentenciaSQL = "SELECT c.id, c.nombre, c.sitio_web, c.instagram, " +
                                      "(u.municipio || ', ' || u.departamento) ubicacion, c.ubicacion_id " +
                                      "FROM cervecerias c JOIN ubicaciones u ON c.ubicacion_id = u.id " +
                                      "WHERE c.id = @cerveceria_id ";

                var resultado = await conexion.QueryAsync<Horario>(sentenciaSQL,
                                    parametrosSentencia);

                if (resultado.Count() > 0)
                    unHorario = resultado.First();
            }

            return unHorario;
        }
    }
}