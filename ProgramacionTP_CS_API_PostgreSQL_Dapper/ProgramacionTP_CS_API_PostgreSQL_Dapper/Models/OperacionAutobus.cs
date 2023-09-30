namespace ProgramacionTB_CS_API_PostgreSQL_Dapper.Models
{
    public class OperacionAutobus
    {
        public int AutobusId { get; set; } = 0;
        public int HorarioId { get; set; } = 0;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otraOperacionAutobus = (OperacionAutobus)obj;

            return AutobusId == otraOperacionAutobus.AutobusId
                && HorarioId == otraOperacionAutobus.HorarioId;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = hash * 5 + AutobusId.GetHashCode();
                hash = hash * 5 + HorarioId.GetHashCode();

                return hash;
            }
        }
    }
}