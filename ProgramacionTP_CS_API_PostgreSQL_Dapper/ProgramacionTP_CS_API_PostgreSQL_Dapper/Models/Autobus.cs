namespace ProgramacionTB_CS_API_PostgreSQL_Dapper.Models
{
    public class Autobus
    {
        public int Id { get; set; } = 0;
        public string Nombre_autobus { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otroAutobus = (Autobus)obj;

            return Id == otroAutobus.Id
                && Nombre_autobus.Equals(otroAutobus.Nombre_autobus);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = hash * 5 + Id.GetHashCode();
                hash = hash * 5 + (Nombre_autobus?.GetHashCode() ?? 0);

                return hash;
            }
        }
    }
}