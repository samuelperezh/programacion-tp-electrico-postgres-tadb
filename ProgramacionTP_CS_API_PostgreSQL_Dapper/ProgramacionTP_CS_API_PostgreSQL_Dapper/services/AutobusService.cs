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
            //Validamos que la Autobus exista con ese Id
            var unAutobus = await _AutobusRepository
                .GetByIdAsync(Autobus_id);

            if (unAutobus.Id == 0)
                throw new AppValidationException($"Autobus no encontrada con el id {Autobus_id}");

            return unAutobus;
        }

        public async Task<IEnumerable<Cerveza>> GetAssociatedBeersAsync(int Autobus_id)
        {
            //Validamos que la Autobus exista con ese Id
            var unAutobus = await _AutobusRepository
                .GetByIdAsync(Autobus_id);

            if (unAutobus.Id == 0)
                throw new AppValidationException($"Autobus no encontrada con el id {Autobus_id}");

            //Si la Autobus existe, validamos que tenga cervezas asociadas
            var cantidadCervezasAsociadas = await _AutobusRepository
                .GetTotalAssociatedBeersAsync(Autobus_id);

            if (cantidadCervezasAsociadas == 0)
                throw new AppValidationException($"No Existen cervezas asociadas a la Autobus {unAutobus.Nombre}");

            return await _AutobusRepository.GetAssociatedBeersAsync(Autobus_id);
        }

        public async Task<Autobus> CreateAsync(Autobus unAutobus)
        {
            //Validamos que la Autobus tenga nombre
            if (unAutobus.Nombre.Length == 0)
                throw new AppValidationException("No se puede insertar una cervecer�a con nombre nulo");

            //Validamos que la Autobus tenga sitio_web
            if (unAutobus.Sitio_Web.Length == 0)
                throw new AppValidationException("No se puede insertar una cervecer�a con Sitio Web nulo");

            //Validamos que la Autobus tenga instagram
            if (unAutobus.Instagram.Length == 0)
                throw new AppValidationException("No se puede insertar una cervecer�a con Instagram nulo");

            //Validamos que la Autobus tenga ubicaci�n
            if (unAutobus.Ubicacion.Length == 0)
                throw new AppValidationException("No se puede insertar una cervecer�a una ubicaci�n nula");

            //Validamos que la Autobus tenga ubicaci�n v�lida
            var ubicacionExistente = await _AutobusRepository
                .GetAssociatedLocationIdAsync(unAutobus.Ubicacion);

            if (ubicacionExistente == 0)
                throw new AppValidationException("No se puede insertar una cervecer�a sin ubicaci�n conocida");

            unAutobus.Ubicacion_Id = ubicacionExistente;

            //Validamos que el nombre no exista previamente
            var AutobusExistente = await _AutobusRepository
                .GetByNameAsync(unAutobus.Nombre);

            if (AutobusExistente.Id != 0)
                throw new AppValidationException($"Ya existe una cervecer�a con el nombre {unAutobus.Nombre}");

            //Validamos que el sitio_web no exista previamente
            AutobusExistente = await _AutobusRepository
                .GetBySitioWebAsync(unAutobus.Sitio_Web);

            if (AutobusExistente.Id != 0)
                throw new AppValidationException($"Ya existe una cervecer�a con el sitio web {unAutobus.Sitio_Web}");

            //Validamos que el instagram no exista previamente
            AutobusExistente = await _AutobusRepository
                .GetByInstagramAsync(unAutobus.Instagram);

            if (AutobusExistente.Id != 0)
                throw new AppValidationException($"Ya existe una cervecer�a con el instagram {unAutobus.Instagram}");

            try
            {
                bool resultadoAccion = await _AutobusRepository
                    .CreateAsync(unAutobus);

                if (!resultadoAccion)
                    throw new AppValidationException("Operaci�n ejecutada pero no gener� cambios en la DB");

                AutobusExistente = await _AutobusRepository
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
                throw new AppValidationException($"Inconsistencia en el Id de la cervecer�a a actualizar. Verifica argumentos");

            //Validamos que la Autobus exista con ese Id
            var AutobusExistente = await _AutobusRepository
                .GetByIdAsync(Autobus_id);

            if (AutobusExistente.Id == 0)
                throw new AppValidationException($"No existe una cervecer�a registrada con el id {unAutobus.Id}");

            //Validamos que la Autobus tenga nombre
            if (unAutobus.Nombre.Length == 0)
                throw new AppValidationException("No se puede actualizar una cervecer�a con nombre nulo");

            //Validamos que el nombre no exista previamente en otra cervecer�a diferente a la que se est� actualizando
            AutobusExistente = await _AutobusRepository
                .GetByNameAsync(unAutobus.Nombre);

            if (unAutobus.Id != AutobusExistente.Id)
                throw new AppValidationException($"Ya existe otra cervecer�a con el nombre {unAutobus.Nombre}. " +
                    $"No se puede Actualizar");

            //Validamos que la Autobus tenga sitio_web
            if (unAutobus.Sitio_Web.Length == 0)
                throw new AppValidationException("No se puede actualizar una cervecer�a con Sitio Web nulo");

            //Validamos que el sitio_web no exista previamente en otra cervecer�a diferente a la que se est� actualizando
            AutobusExistente = await _AutobusRepository
                .GetBySitioWebAsync(unAutobus.Sitio_Web);

            if (unAutobus.Id != AutobusExistente.Id)
                throw new AppValidationException($"Ya existe otra cervecer�a con el sitio web {unAutobus.Sitio_Web}. " +
                    $"No se puede Actualizar");

            //Validamos que la Autobus tenga instagram
            if (unAutobus.Instagram.Length == 0)
                throw new AppValidationException("No se puede actualizar una cervecer�a con Instagram nulo");

            //Validamos que el instagram no exista previamente en otra cervecer�a diferente a la que se est� actualizando
            AutobusExistente = await _AutobusRepository
                .GetByInstagramAsync(unAutobus.Instagram);

            if (unAutobus.Id != AutobusExistente.Id)
                throw new AppValidationException($"Ya existe otra cervecer�a con el instagram {unAutobus.Instagram}. " +
                    $"No se puede Actualizar");

            //Validamos que la Autobus tenga ubicaci�n
            if (unAutobus.Ubicacion.Length == 0)
                throw new AppValidationException("No se puede actualizar una cervecer�a con ubicaci�n nula");

            //Validamos que la Autobus tenga ubicaci�n v�lida
            var ubicacionExistente = await _AutobusRepository
                .GetAssociatedLocationIdAsync(unAutobus.Ubicacion);

            if (ubicacionExistente == 0)
                throw new AppValidationException("No se puede actualizar una cervecer�a sin ubicaci�n conocida");

            unAutobus.Ubicacion_Id = ubicacionExistente;

            //Validamos que haya al menos un cambio en las propiedades
            if (unAutobus.Equals(AutobusExistente))
                throw new AppValidationException("No hay cambios en los atributos de la cervecer�a. No se realiza actualizaci�n.");

            try
            {
                bool resultadoAccion = await _AutobusRepository
                    .UpdateAsync(unAutobus);

                if (!resultadoAccion)
                    throw new AppValidationException("Operaci�n ejecutada pero no gener� cambios en la DB");

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

            // Validamos que la Autobus no tenga asociadas cervezas
            var cantidadCervezasAsociadas = await _AutobusRepository
                .GetTotalAssociatedBeersAsync(AutobusExistente.Id);

            if (cantidadCervezasAsociadas > 0)
                throw new AppValidationException($"Existen {cantidadCervezasAsociadas} cervezas " +
                    $"asociadas a {AutobusExistente.Nombre}. No se puede eliminar");

            //Si existe y no tiene cervezas asociadas, se puede eliminar
            try
            {
                bool resultadoAccion = await _AutobusRepository
                    .DeleteAsync(AutobusExistente);

                if (!resultadoAccion)
                    throw new AppValidationException("Operaci�n ejecutada pero no gener� cambios en la DB");
            }
            catch (DbOperationException error)
            {
                throw error;
            }
        }
    }
}
