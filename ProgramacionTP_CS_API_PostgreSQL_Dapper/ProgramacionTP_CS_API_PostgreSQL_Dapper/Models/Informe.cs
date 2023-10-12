namespace ProgramacionTP_CS_API_PostgreSQL_Dapper.Models
{
    public class Informe
    {
        public int Horarios { get; set; } = 0;
        public int Autobuses { get; set; } =0;
        public int Cargadores { get; set; } = 0;
        public float Operacion_autobuses { get; set; } = 0;
        public float Utilizacion_cargadores { get; set; } = 0;
    }
}
