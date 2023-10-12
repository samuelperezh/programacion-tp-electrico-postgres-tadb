using ProgramacionTB_CS_API_PostgreSQL_Dapper.Helpers;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces;
using ProgramacionTB_CS_API_PostgreSQL_Dapper.Models;

namespace ProgramacionTB_CS_API_PostgreSQL_Dapper.Services
{
    public class CargadorService
    {
        private readonly ICargadorRepository _cargadorRepository;

        public CargadorService(ICargadorRepository cargadorRepository)
        {
            _cargadorRepository = cargadorRepository;
        }

        public async Task<IEnumerable<Cargador>> GetAllAsync()
        {
            return await _cargadorRepository
                .GetAllAsync();
        }

        public async Task<Cargador> GetByIdAsync(int cargador_id)
        {
            // Validamos que el cargador exista con ese Id
            var unCargador = await _cargadorRepository
                .GetByIdAsync(cargador_id);

            if (unCargador.Id == 0)
                throw new AppValidationException($"Cargador no encontrado con el id {cargador_id}");

            return unCargador;
        }

        public async Task<Cargador> CreateAsync(Cargador unCargador)
        {
            // Validamos que el nombre no exista previamente
            var cargadorExistente = await _cargadorRepository
                .GetByNameAsync(unCargador.Nombre_cargador);

            if (cargadorExistente.Id != 0)
                throw new AppValidationException($"Ya existe un cargador con el nombre {cargadorExistente.Nombre_cargador}");
            try
            {
                bool resultadoAccion = await _cargadorRepository
                    .CreateAsync(unCargador);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                unCargador = await _cargadorRepository
                    .GetByNameAsync(unCargador.Nombre_cargador!);
            }
            catch (DbOperationException error)
            {
                throw error;
            }

            return cargadorExistente;
        }

        public async Task<Cargador> UpdateAsync(int cargador_id, Cargador unCargador)
        {
            // Validamos que los parámetros sean consistentes
            if (cargador_id != unCargador.Id)
                throw new AppValidationException($"Inconsistencia en el Id del cargador a actualizar. Verifica argumentos");

            // Validamos que el cargador exista con ese Id
            var cargadorExistente = await _cargadorRepository
                .GetByIdAsync(cargador_id);

            if (cargadorExistente.Id == 0)
                throw new AppValidationException($"No existe un cargador registrado con el id {unCargador.Id}");

            // Validamos que el cargador tenga nombre
            if (unCargador.Nombre_cargador.Length == 0)
                throw new AppValidationException("No se puede actualizar un cargador con nombre nulo");

            // Validamos que el nombre no exista previamente en otro cargador diferente al que se está actualizando
            cargadorExistente = await _cargadorRepository
                .GetByNameAsync(unCargador.Nombre_cargador);

            if (unCargador.Id != cargadorExistente.Id)
                throw new AppValidationException($"Ya existe otro autobus con el nombre {unCargador.Nombre_cargador}. " +
                    $"No se puede Actualizar");

            // Validamos que haya al menos un cambio en las propiedades
            if (unCargador.Equals(cargadorExistente))
                throw new AppValidationException("No hay cambios en los atributos del cargador. No se realiza actualización.");

            try
            {
                bool resultadoAccion = await _cargadorRepository
                    .UpdateAsync(unCargador);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                cargadorExistente = await _cargadorRepository
                    .GetByNameAsync(unCargador.Nombre_cargador!);
            }
            catch (DbOperationException error)
            {
                throw error;
            }

            return cargadorExistente;
        }

        public async Task DeleteAsync(int cargador_id)
        {
            // Validamos que el cargador a eliminar si exista con ese Id
            var cargadorExistente = await _cargadorRepository
                .GetByIdAsync(cargador_id);

            if (cargadorExistente.Id == 0)
                throw new AppValidationException($"No existe un cargador con el Id {cargador_id} que se pueda eliminar");

            // Validamos que el cargador no tenga asociadas utilizaciones
            var cantidadUtilizacionesAsociados = await _cargadorRepository
                .GetTotalAssociatedChargerUtilizationAsync(cargadorExistente.Id);

            if (cantidadUtilizacionesAsociados > 0)
                throw new AppValidationException($"Existen {cantidadUtilizacionesAsociados} cargadores " +
                    $"asociados a {cargadorExistente.Nombre_cargador}. No se puede eliminar");

            // Si existe y no tiene utilizaciones asociadas, se puede eliminar
            try
            {
                bool resultadoAccion = await _cargadorRepository
                    .DeleteAsync(cargadorExistente);

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