using ProgramacionTB_CS_API_PostgreSQL_Dapper.Helpers;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces;
using ProgramacionTB_CS_API_PostgreSQL_Dapper.Models;

namespace ProgramacionTB_CS_API_PostgreSQL_Dapper.Services
{
    public class OperacionAutobusService
    {
        private readonly IOperacionAutobusRepository _OperacionAutobusRepository;

        public OperacionAutobusService(IOperacionAutobusRepository OperacionAutobusRepository)
        {
            _OperacionAutobusRepository = OperacionAutobusRepository;
        }

        public async Task<IEnumerable<OperacionAutobus>> GetAllAsync()
        {
            return await _OperacionAutobusRepository
                .GetAllAsync();
        }
        public async Task<OperacionAutobus> CreateAsync(OperacionAutobus unaOperacionAutobus)
        {
            //PREGUNTARLE AL PROFE SI NO ES NECESARIO VALIDAR QUE EL ID DE LA OPERACION AUTOBUS NO EXISTA PREVIAMENTE
            //Validamos que el autobus_id no exista previamente
            //var OperacionAutobusExistente = await _OperacionAutobusRepository
              //  .GetByNameAsync(unaOperacionAutobus.Nombre);

            //if (OperacionAutobusExistente.Id != 0)
              //  throw new AppValidationException($"Ya existe una operacion de un autobus con el nombre {CargadorExistente.Nombre}");
            try
            {
                bool resultadoAccion = await _OperacionAutobusRepository
                    .CreateAsync(unaOperacionAutobus);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                unaOperacionAutobus = await _OperacionAutobusRepository;
                    //.GetByNameAsync(unCargador.Nombre!);
            }
            catch (DbOperationException error)
            {
                throw error;
            }

            return OperacionAutobusExistente;
        }

        public async Task<OperacionAutobus> UpdateAsync(int _id, Cargador unCargador)
        {
            //Validamos que los parametros sean consistentes
            if (Cargador_id != unCargador.Id)
                throw new AppValidationException($"Inconsistencia en el Id del cargador a actualizar. Verifica argumentos");

            //Validamos que el cargador exista con ese Id
            var CargadorExistente = await _CargadorRepository
                .GetByIdAsync(Cargador_id);

            if (CargadorExistente.Id == 0)
                throw new AppValidationException($"No existe un Cargador registrado con el id {unCargador.Id}");

            //Validamos que la Autobus tenga nombre
            if (unCargador.Nombre.Length == 0)
                throw new AppValidationException("No se puede actualizar un Cargador con nombre nulo");

            //Validamos que el nombre no exista previamente en otro Cargador diferente a la que se está actualizando
            CargadorExistente = await _CargadorRepository
                .GetByNameAsync(unCargador.Nombre);

            if (unCargador.Id != CargadorExistente.Id)
                throw new AppValidationException($"Ya existe otro autobus con el nombre {unCargador.Nombre}. " +
                    $"No se puede Actualizar");

            //Validamos que haya al menos un cambio en las propiedades
            if (unCargador.Equals(CargadorExistente))
                throw new AppValidationException("No hay cambios en los atributos del Cargador. No se realiza actualización.");

            try
            {
                bool resultadoAccion = await _CargadorRepository
                    .UpdateAsync(unCargador);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                CargadorExistente = await _CargadorRepository
                    .GetByNameAsync(unCargador.Nombre!);
            }
            catch (DbOperationException error)
            {
                throw error;
            }

            return CargadorExistente;
        }

        public async Task DeleteAsync(int Cargador_id)
        {
            // validamos que el Cargador a eliminar si exista con ese Id
            var CargadorExistente = await _CargadorRepository
                .GetByIdAsync(Cargador_id);

            if (CargadorExistente.Id == 0)
                throw new AppValidationException($"No existe un Cargador con el Id {Cargador_id} que se pueda eliminar");

            //Si existe se puede eliminar
            try
            {
                bool resultadoAccion = await _CargadorRepository
                    .DeleteAsync(CargadorExistente);

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