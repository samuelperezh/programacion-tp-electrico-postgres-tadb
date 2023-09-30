namespace ProgramacionTB_CS_API_PostgreSQL_Dapper.Models
{
    public class UtilizacionCargador
    {
        public int CargadorId { get; set; } = 0;
        public int AutobusId { get; set; } = 0;
        public int HorarioId { get; set; } = 0;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otraUtilizacion = (UtilizacionCargador)obj;

            return CargadorId == otraUtilizacion.CargadorId
                && AutobusId == otraUtilizacion.AutobusId
                && HorarioId == otraUtilizacion.HorarioId;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = hash * 5 + CargadorId.GetHashCode();
                hash = hash * 5 + AutobusId.GetHashCode();
                hash = hash * 5 + HorarioId.GetHashCode();

                return hash;
            }
        }
    }
}