using ProgramacionTB_CS_API_PostgreSQL_Dapper.Helpers;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces;
using ProgramacionTB_CS_API_PostgreSQL_Dapper.Models;

namespace ProgramacionTB_CS_API_PostgreSQL_Dapper.Services
{
    public class AutobusService
    {
        private readonly IAutobusRepository _AutobusRepository;

        public AutobusService(IAutobusRepository AutobusRepository)
        {
            _AutobusRepository = AutobusRepository;
        }

        public async Task<IEnumerable<Autobus>> GetAllAsync()
        {
            return await _AutobusRepository
                .GetAllAsync();
        }

        public async Task<Autobus> GetByIdAsync(int Autobus_id)
        {
            //Validamos que el Autobus exista con ese Id
            var unAutobus = await _AutobusRepository
                .GetByIdAsync(Autobus_id);

            if (unAutobus.Id == 0)
                throw new AppValidationException($"Autobus no encontrado con el id {Autobus_id}");

            return unAutobus;
        }

        public async Task<Autobus> CreateAsync(Autobus unAutobus)
        {
            //Validamos que el nombre no exista previamente
            var AutobusExistente = await _AutobusRepository
                .GetByNameAsync(unAutobus.Nombre);

            if (AutobusExistente.Id != 0)
                throw new AppValidationException($"Ya existe un autobus con el nombre {AutobusExistente.Nombre}");
            try
            {
                bool resultadoAccion = await _AutobusRepository
                    .CreateAsync(unAutobus);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                unAutobus = await _AutobusRepository
                    .GetByNameAsync(unAutobus.Nombre!);
            }
            catch (DbOperationException error)
            {
                throw error;
            }

            return AutobusExistente;
        }

        public async Task<Autobus> UpdateAsync(int Autobus_id, Autobus unAutobus)
        {
            //Validamos que los parametros sean consistentes
            if (Autobus_id != unAutobus.Id)
                throw new AppValidationException($"Inconsistencia en el Id del autobus a actualizar. Verifica argumentos");

            //Validamos que la Autobus exista con ese Id
            var AutobusExistente = await _AutobusRepository
                .GetByIdAsync(Autobus_id);

            if (AutobusExistente.Id == 0)
                throw new AppValidationException($"No existe un autobus registrada con el id {unAutobus.Id}");

            //Validamos que la Autobus tenga nombre
            if (unAutobus.Nombre.Length == 0)
                throw new AppValidationException("No se puede actualizar un autobus con nombre nulo");

            //Validamos que el nombre no exista previamente en otro autobus diferente a la que se está actualizando
            AutobusExistente = await _AutobusRepository
                .GetByNameAsync(unAutobus.Nombre);

            if (unAutobus.Id != AutobusExistente.Id)
                throw new AppValidationException($"Ya existe otro autobus con el nombre {unAutobus.Nombre}. " +
                    $"No se puede Actualizar");

            //Validamos que haya al menos un cambio en las propiedades
            if (unAutobus.Equals(AutobusExistente))
                throw new AppValidationException("No hay cambios en los atributos del autobus. No se realiza actualización.");

            try
            {
                bool resultadoAccion = await _AutobusRepository
                    .UpdateAsync(unAutobus);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                AutobusExistente = await _AutobusRepository
                    .GetByNameAsync(unAutobus.Nombre!);
            }
            catch (DbOperationException error)
            {
                throw error;
            }

            return AutobusExistente;
        }

        public async Task DeleteAsync(int Autobus_id)
        {
            // validamos que el Autobus a eliminar si exista con ese Id
            var AutobusExistente = await _AutobusRepository
                .GetByIdAsync(Autobus_id);

            if (AutobusExistente.Id == 0)
                throw new AppValidationException($"No existe una Autobus con el Id {Autobus_id} que se pueda eliminar");

            //Si existe se puede eliminar
            try
            {
                bool resultadoAccion = await _AutobusRepository
                    .DeleteAsync(AutobusExistente);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");
            }
            catch (DbOperationException error)
            {
                throw error;
            }
        }
    }
}
