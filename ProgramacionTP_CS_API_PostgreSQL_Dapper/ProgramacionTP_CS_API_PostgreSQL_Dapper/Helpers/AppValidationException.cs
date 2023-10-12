/*
 AppValidationException:
 Excepcion creada para enviar mensajes relacionados 
 con la validaci�n en todas las operaciones CRUD de la aplicaci�n
*/


namespace ProgramacionTB_CS_API_PostgreSQL_Dapper.Helpers
{
    public class AppValidationException : Exception
    {
        public AppValidationException(string message) : base(message) { }
    }
}