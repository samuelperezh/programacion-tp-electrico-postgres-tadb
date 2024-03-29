﻿using ProgramacionTB_CS_API_PostgreSQL_Dapper.Models;

namespace ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces
{
    public interface IOperacionAutobusRepository
    {

        public Task<IEnumerable<OperacionAutobus>> GetAllAsync();
        public Task<OperacionAutobus> GetByOperationAsync(int autobus_id, int horario_id);
        public Task<string> GetAutobusStateAsync(int horario_id, int autobus_id);

        public Task<bool> CreateAsync(OperacionAutobus unaOperacionAutobus);
        public Task<bool> UpdateAsync(OperacionAutobus unaOperacionAutobus);
        public Task<bool> DeleteAsync(OperacionAutobus unaOperacionAutobus);
    }
}
