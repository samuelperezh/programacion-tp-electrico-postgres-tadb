namespace ProgramacionTB_CS_API_PostgreSQL_Dapper.Models
{
    public class Autobuses
    {
        public int Id { get; set; } = 0;
        public string Autobus { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otroAutobus = (Autobuses)obj;

            return Id == otroAutobus.Id
                && Autobus.Equals(otroAutobus.Autobus);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = hash * 5 + Id.GetHashCode();
                hash = hash * 5 + (Autobus?.GetHashCode() ?? 0);

                return hash;
            }
        }
    }
}