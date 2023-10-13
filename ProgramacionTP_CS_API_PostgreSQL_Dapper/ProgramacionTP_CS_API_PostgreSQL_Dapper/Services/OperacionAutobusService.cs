using ProgramacionTB_CS_API_PostgreSQL_Dapper.Helpers;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces;
using ProgramacionTB_CS_API_PostgreSQL_Dapper.Models;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Repositories;
using System.Security.AccessControl;

namespace ProgramacionTB_CS_API_PostgreSQL_Dapper.Services
{
    public class OperacionAutobusService
    {
        private readonly IOperacionAutobusRepository _operacionAutobusRepository;
        private readonly IAutobusRepository _autobusRepository;
        private readonly IHorarioRepository _horarioRepository;

        public OperacionAutobusService(IOperacionAutobusRepository operacionAutobusRepository,
                                       IAutobusRepository autobusRepository,
                                       IHorarioRepository horarioRepository)
        {
            _operacionAutobusRepository = operacionAutobusRepository;
            _autobusRepository = autobusRepository;
            _horarioRepository = horarioRepository;
        }

        public async Task<IEnumerable<OperacionAutobus>> GetAllAsync()
        {
            return await _operacionAutobusRepository
                .GetAllAsync();
        }

        public async Task<OperacionAutobus> CreateAsync(OperacionAutobus unaOperacionAutobus)
        {
            // Validamos que el autobus exista con ese Id
            var autobusExistente = await _autobusRepository
                .GetByIdAsync(unaOperacionAutobus.Autobus_id);

            if (autobusExistente.Id == 0)
                throw new AppValidationException($"El autobus con id {autobusExistente.Id} no se encuentra registrado");

            // Validamos que el horario exista con ese Id
            var horarioExistente = await _horarioRepository
                .GetByIdAsync(unaOperacionAutobus.Horario_id);

            if (horarioExistente.Id == 0)
                throw new AppValidationException($"El horario con id {autobusExistente.Id} no se encuentra registrado");

            //Validamos que no exista previamente
            var operacionAutobusExistente = await _operacionAutobusRepository
                .GetByOperationAsync(unaOperacionAutobus.Autobus_id, unaOperacionAutobus.Horario_id);

            if (operacionAutobusExistente.Autobus_id != 0) 
                throw new AppValidationException($"Ya existe una operaciín con el autobus {operacionAutobusExistente.Autobus_id} en el horario {operacionAutobusExistente.Horario_id}");

            // Se valida que el autobus no haya estado operando 4 veces sin cargarse
            int maxOperandoCount = 6;
            int operandoCount = 0;

            for (int hora = 0; hora <= unaOperacionAutobus.Horario_id; hora++)
            {
                string estado = await _operacionAutobusRepository.GetAutobusStateAsync(hora, unaOperacionAutobus.Autobus_id);

                if (estado == "Operando")
                {
                    operandoCount++;
                }
                else if (estado == "Cargando")
                {
                    operandoCount = 0; // Reinicia la cuenta si se encuentra "Cargando"
                }

                if (operandoCount >= maxOperandoCount)
                {
                    throw new AppValidationException("El autobus ha estado operando más de 4 horas sin ser cargado, no se puede insertar.");
                }
            }


            try
            {
                bool resultadoAccion = await _operacionAutobusRepository
                    .CreateAsync(unaOperacionAutobus);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                unaOperacionAutobus = await _operacionAutobusRepository
                    .GetByOperationAsync(unaOperacionAutobus.Autobus_id!, unaOperacionAutobus.Horario_id!);
            }
            catch (DbOperationException error)
            {
                throw error;
            }

            return operacionAutobusExistente;
        }

        public async Task<OperacionAutobus> UpdateAsync(int autobus_id, int horario_id,OperacionAutobus unaOperacionAutobus)
        {
            //Validamos que los parametros sean consistentes
            if (autobus_id != unaOperacionAutobus.Autobus_id && horario_id != unaOperacionAutobus.Horario_id)
                throw new AppValidationException($"Inconsistencia en el id del autobus y del horario a actualizar. Verifica argumentos");

            // Validamos que el autobus exista con ese Id
            var autobusExistente = await _autobusRepository
                .GetByIdAsync(autobus_id);

            if (autobusExistente.Id == 0)
                throw new AppValidationException($"El autobus con id {autobusExistente.Id} no se encuentra registrado");

            // Validamos que el horario exista con ese Id
            var horarioExistente = await _horarioRepository
                .GetByIdAsync(horario_id);

            if (horarioExistente.Id == 0)
                throw new AppValidationException($"El horario con id {autobusExistente.Id} no se encuentra registrado");

            // Validamos que exista previamente
            var operacionAutobusExistente = await _operacionAutobusRepository
                .GetByOperationAsync(unaOperacionAutobus.Autobus_id, unaOperacionAutobus.Horario_id);

            if (operacionAutobusExistente.Autobus_id == 0)
                throw new AppValidationException($"No existe una operacion con el autobus {operacionAutobusExistente.Autobus_id} en el horario {operacionAutobusExistente.Horario_id}");

            //Validamos que haya al menos un cambio en las propiedades
            if (unaOperacionAutobus.Equals(operacionAutobusExistente))
                throw new AppValidationException("No hay cambios en los atributos de la operación. No se realiza actualización.");

            try
            {
                bool resultadoAccion = await _operacionAutobusRepository
                    .UpdateAsync(unaOperacionAutobus);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                operacionAutobusExistente = await _operacionAutobusRepository
                    .GetByOperationAsync(unaOperacionAutobus.Autobus_id!,unaOperacionAutobus.Horario_id!);
            }
            catch (DbOperationException error)
            {
                throw error;
            }

            return operacionAutobusExistente;
        }

        public async Task<OperacionAutobus> DeleteAsync(int autobus_id, int horario_id)
        { 
           
            // Validamos que exista previamente
            var operacionAutobusExistente = await _operacionAutobusRepository
                .GetByOperationAsync(autobus_id, horario_id);

            if (operacionAutobusExistente.Autobus_id == 0)
                throw new AppValidationException($"No existe una operación con el autobus {operacionAutobusExistente.Autobus_id} en el horario {operacionAutobusExistente.Horario_id} para eliminar");
           
            //Si existe y no tiene operaciones asociadas, se puede eliminar
            try
            {
                bool resultadoAccion = await _operacionAutobusRepository
                    .DeleteAsync(operacionAutobusExistente);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");
            }
            catch (DbOperationException error)
            {
                throw error;
            }

            return operacionAutobusExistente;
        }

        internal Task GetByOperationAsync(int v1, int autobus_id, int v2, int horario_id)
        {
            throw new NotImplementedException();
        }
    }
}