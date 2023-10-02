using ProgramacionTB_CS_API_PostgreSQL_Dapper.Helpers;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces;
using ProgramacionTB_CS_API_PostgreSQL_Dapper.Models;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Repositories;

namespace ProgramacionTB_CS_API_PostgreSQL_Dapper.Services
{
    public class AutobusService
    {
        private readonly IAutobusRepository _autobusRepository;

        public AutobusService(IAutobusRepository autobusRepository)
        {
            _autobusRepository = autobusRepository;
        }

        public async Task<IEnumerable<Autobus>> GetAllAsync()
        {
            return await _autobusRepository
                .GetAllAsync();
        }

        public async Task<Autobus> GetByIdAsync(int autobus_id)
        {
            // Validamos que el autobus exista con ese Id
            var unAutobus = await _autobusRepository
                .GetByIdAsync(autobus_id);

            if (unAutobus.Id == 0)
                throw new AppValidationException($"Autobus no encontrado con el id {autobus_id}");

            return unAutobus;
        }

        public async Task<Autobus> CreateAsync(Autobus unAutobus)
        {
            // Validamos que el nombre no exista previamente
            var autobusExistente = await _autobusRepository
                .GetByNameAsync(unAutobus.Nombre);

            if (autobusExistente.Id != 0)
                throw new AppValidationException($"Ya existe un autobus con el nombre {autobusExistente.Nombre}");
            try
            {
                bool resultadoAccion = await _autobusRepository
                    .CreateAsync(unAutobus);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                unAutobus = await _autobusRepository
                    .GetByNameAsync(unAutobus.Nombre!);
            }
            catch (DbOperationException error)
            {
                throw error;
            }

            return autobusExistente;
        }

        public async Task<Autobus> UpdateAsync(int autobus_id, Autobus unAutobus)
        {
            // Validamos que los parámetros sean consistentes
            if (autobus_id != unAutobus.Id)
                throw new AppValidationException($"Inconsistencia en el Id del autobus a actualizar. Verifica argumentos");

            // Validamos que el autobus exista con ese Id
            var autobusExistente = await _autobusRepository
                .GetByIdAsync(autobus_id);

            if (autobusExistente.Id == 0)
                throw new AppValidationException($"No existe un autobus registrado con el id {unAutobus.Id}");

            // Validamos que el autobus tenga nombre
            if (unAutobus.Nombre.Length == 0)
                throw new AppValidationException("No se puede actualizar un autobus con nombre nulo");

            // Validamos que el nombre no exista previamente en otro autobus diferente al que se está actualizando
            autobusExistente = await _autobusRepository
                .GetByNameAsync(unAutobus.Nombre);

            if (unAutobus.Id != autobusExistente.Id)
                throw new AppValidationException($"Ya existe otro autobus con el nombre {unAutobus.Nombre}. " +
                    $"No se puede Actualizar");

            // Validamos que haya al menos un cambio en las propiedades
            if (unAutobus.Equals(autobusExistente))
                throw new AppValidationException("No hay cambios en los atributos del autobus. No se realiza actualización.");

            try
            {
                bool resultadoAccion = await _autobusRepository
                    .UpdateAsync(unAutobus);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                autobusExistente = await _autobusRepository
                    .GetByNameAsync(unAutobus.Nombre!);
            }
            catch (DbOperationException error)
            {
                throw error;
            }

            return autobusExistente;
        }

        public async Task DeleteAsync(int autobus_id)
        {
            // Validamos que el autobus a eliminar si exista con ese Id
            var autobusExistente = await _autobusRepository
                .GetByIdAsync(autobus_id);

            if (autobusExistente.Id == 0)
                throw new AppValidationException($"No existe un autobus con el Id {autobus_id} que se pueda eliminar");

            // Validamos que el autobus no tenga asociadas utilizaciones de cargadores
            var cantidadUtilizacionCargadoresAsociados = await _autobusRepository
                .GetTotalAssociatedChargerUtilizationAsync(autobusExistente.Id);

            if (cantidadUtilizacionCargadoresAsociados > 0)
                throw new AppValidationException($"Existen {cantidadUtilizacionCargadoresAsociados} utilizaciones de cargadores " +
                    $"asociados a {autobusExistente.Nombre}. No se puede eliminar");

            // Validamos que el autobus no tenga asociadas operaciones
            var cantidadOperacionesAutobusAsociados = await _autobusRepository
                .GetTotalAssociatedAutobusOperationAsync(autobusExistente.Id);

            if (cantidadOperacionesAutobusAsociados > 0)
                throw new AppValidationException($"Existen {cantidadOperacionesAutobusAsociados} operaciones de autobuses " +
                    $"asociados a {autobusExistente.Nombre}. No se puede eliminar");

            // Si existe y no tiene utilización de cargadores y operaciones asociadas, se puede eliminar
            try
            {
                bool resultadoAccion = await _autobusRepository
                    .DeleteAsync(autobusExistente);

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
