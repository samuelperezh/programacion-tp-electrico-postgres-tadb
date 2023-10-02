namespace ProgramacionTB_CS_API_PostgreSQL_Dapper.Models
{
    public class OperacionAutobus
    {
        public int Autobus_id { get; set; } = 0;
        public int Horario_id { get; set; } = 0;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otraOperacionAutobus = (OperacionAutobus)obj;

            return Autobus_id == otraOperacionAutobus.Autobus_id
                && Horario_id == otraOperacionAutobus.Horario_id;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = hash * 5 + Autobus_id.GetHashCode();
                hash = hash * 5 + Horario_id.GetHashCode();

                return hash;
            }
        }
    }
}