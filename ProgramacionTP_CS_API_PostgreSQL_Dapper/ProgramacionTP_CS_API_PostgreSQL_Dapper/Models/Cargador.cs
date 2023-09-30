﻿namespace ProgramacionTB_CS_API_PostgreSQL_Dapper.Models
{
    public class Cargador
    {
        public int Id { get; set; } = 0;
        public string Nombre { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otroCargador = (Cargador)obj;

            return Id == otroCargador.Id
                && Nombre.Equals(otroCargador.Nombre);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = hash * 5 + Id.GetHashCode();
                hash = hash * 5 + (Nombre?.GetHashCode() ?? 0);

                return hash;
            }
        }
    }
}