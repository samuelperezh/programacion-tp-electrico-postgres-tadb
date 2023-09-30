/*
 DbOperationException:
 Excepcion creada para enviar mensajes relacionados 
 con las acciones a nivel de base de datos en todas
 las operaciones CRUD de la aplicaci�n
*/

namespace ProgramacionTB_CS_API_PostgreSQL_Dapper.Helpers
{
    public class DbOperationException : Exception
    {
        public DbOperationException(string message) : base(message) { }
    }
}