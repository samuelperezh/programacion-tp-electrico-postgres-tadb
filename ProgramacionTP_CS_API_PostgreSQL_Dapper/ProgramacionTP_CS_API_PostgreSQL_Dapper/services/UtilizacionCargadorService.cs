using ProgramacionTB_CS_API_PostgreSQL_Dapper.Helpers;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces;
using ProgramacionTB_CS_API_PostgreSQL_Dapper.Models;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Repositories;

namespace ProgramacionTB_CS_API_PostgreSQL_Dapper.Services
{
    public class UtilizacionCargadorService
    {
        private readonly IUtilizacionCargadorRepository _utilizacionCargadorRepository;
        private readonly ICargadorRepository _cargadorRepository;
        private readonly IAutobusRepository _autobusRepository;
        private readonly IHorarioRepository _horarioRepository;

        public UtilizacionCargadorService(IUtilizacionCargadorRepository utilizacionCargadorRepository,
                                          ICargadorRepository cargadorRepository,
                                          IAutobusRepository autobusRepository,
                                          IHorarioRepository horarioRepository)
        {
            _utilizacionCargadorRepository = utilizacionCargadorRepository;
            _cargadorRepository = cargadorRepository;
            _autobusRepository = autobusRepository;
            _horarioRepository = horarioRepository;
        }

        public async Task<IEnumerable<UtilizacionCargador>> GetAllAsync()
        {
            return await _utilizacionCargadorRepository
                .GetAllAsync();
        }
        
        public async Task<UtilizacionCargador> CreateAsync(UtilizacionCargador unaUtilizacionCargador)
        {
            //Validamos que el cargador exista con ese Id
            var cargadorExistente = await _cargadorRepository
                .GetByIdAsync(unaUtilizacionCargador.Cargador_id);

            if (cargadorExistente.Id == 0)
                throw new AppValidationException($"El cargador con id {cargadorExistente.Id} no se encuentra registrado");

            //Validamos que el autobus exista con ese Id
            var autobusExistente = await _autobusRepository
                .GetByIdAsync(unaUtilizacionCargador.Autobus_id);

            if (autobusExistente.Id == 0)
                throw new AppValidationException($"El autobus con id {autobusExistente.Id} no se encuentra registrado");

            //Validamos que el horario exista con ese Id
            var horarioExistente = await _horarioRepository
                .GetByIdAsync(unaUtilizacionCargador.Horario_id);

            if (horarioExistente.Id == 0)
                throw new AppValidationException($"El horario {horarioExistente.Id} no se encuentra registrado");

            //Validamos que la utilización no exista previamente
            var utilizacionCargadorExistente = await _utilizacionCargadorRepository
                .GetByUtilizationAsync(unaUtilizacionCargador.Cargador_id, unaUtilizacionCargador.Autobus_id, unaUtilizacionCargador.Horario_id);

            if (utilizacionCargadorExistente.Cargador_id != 0)
                throw new AppValidationException($"Ya existe una utilización de cargador en el autobus {utilizacionCargadorExistente.Autobus_id}, en el cargador {utilizacionCargadorExistente.Cargador_id}, en la hora {utilizacionCargadorExistente.Horario_id}");

            try
            {
                bool resultadoAccion = await _utilizacionCargadorRepository
                    .CreateAsync(unaUtilizacionCargador);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                unaUtilizacionCargador = await _utilizacionCargadorRepository
                    .GetByUtilizationAsync(unaUtilizacionCargador.Cargador_id!, unaUtilizacionCargador.Autobus_id!, unaUtilizacionCargador.Horario_id!);
            }
            catch (DbOperationException error)
            {
                throw error;
            }

            return utilizacionCargadorExistente;
        }

        public async Task<UtilizacionCargador> UpdateAsync(int cargador_id, int autobus_id, int horario_id,UtilizacionCargador unaUtilizacionCargador)
        {
            //Validamos que el cargador exista con ese Id
            var cargadorExistente = await _cargadorRepository
                .GetByIdAsync(cargador_id);

            if (cargadorExistente.Id == 0)
                throw new AppValidationException($"El cargador con id {cargadorExistente.Id} no se encuentra registrado");

            // Validamos que el autobus exista con ese Id
            var autobusExistente = await _autobusRepository
                .GetByIdAsync(autobus_id);

            if (autobusExistente.Id == 0)
                throw new AppValidationException($"El autobus con id {autobusExistente.Id} no se encuentra registrado");

            //Validamos que el horario exista con ese Id
            var horarioExistente = await _horarioRepository
                .GetByIdAsync(horario_id);

            if (horarioExistente.Id == 0)
                throw new AppValidationException($"El horario {horarioExistente.Id} no se encuentra registrado");

            //Validamos que la utilización exista previamente
            var utilizacionCargadorExistente = await _utilizacionCargadorRepository
              .GetByUtilizationAsync(unaUtilizacionCargador.Cargador_id, unaUtilizacionCargador.Autobus_id, unaUtilizacionCargador.Horario_id);

            if (utilizacionCargadorExistente.Cargador_id == 0)
                throw new AppValidationException($"No existe una utilización de cargador en el autobus {utilizacionCargadorExistente.Autobus_id}, en el cargador {utilizacionCargadorExistente.Cargador_id}, en la hora {utilizacionCargadorExistente.Horario_id}");

            //Validamos que haya al menos un cambio en las propiedades
            if (unaUtilizacionCargador.Equals(utilizacionCargadorExistente))
                throw new AppValidationException("No hay cambios en los atributos de la utilización cargador. No se realiza actualización.");

            try
            {
                bool resultadoAccion = await _utilizacionCargadorRepository
                    .UpdateAsync(unaUtilizacionCargador);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                utilizacionCargadorExistente = await _utilizacionCargadorRepository
                    .GetByUtilizationAsync(unaUtilizacionCargador.Cargador_id!, unaUtilizacionCargador.Autobus_id!, unaUtilizacionCargador.Horario_id!);
            }
            catch (DbOperationException error)
            {
                throw error;
            }

            return utilizacionCargadorExistente;
        }

        public async Task DeleteAsync(int cargador_id, int autobus_id, int horario_id)
        {
            // Validamos que la utilización del cargador a eliminar si exista previamente
            var utilizacionCargadorExistente = await _utilizacionCargadorRepository
                .GetByUtilizationAsync(cargador_id, autobus_id, horario_id);

            if (utilizacionCargadorExistente.Cargador_id == 0)
                throw new AppValidationException($"No existe una utilización de cargador en el autobus {utilizacionCargadorExistente.Autobus_id}, en el cargador {utilizacionCargadorExistente.Cargador_id}, en la hora {utilizacionCargadorExistente.Horario_id}");

            //Si existe se puede eliminar
            try
            {
                bool resultadoAccion = await _utilizacionCargadorRepository
                    .DeleteAsync(utilizacionCargadorExistente);

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