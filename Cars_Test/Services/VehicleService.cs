using AutoMapper;
using Cars_Test.Data.Entities;
using Cars_Test.Data.Repository.Contracts;
using Cars_Test.DTO;
using Cars_Test.DTO.Vehicle;
using Cars_Test.Services.Base;

namespace Cars_Test.Services
{
    /// <summary>
    /// Сервис для работы с данными траснпорта
    /// </summary>
    public class VehicleService : IVehicleService
    {
        private readonly IVehiclesRepository _vehicleRepository;
        private readonly IEmployeesRepository _employeeRepository;
        private readonly ILogger<VehicleService> _logger;
        private readonly IMapper _mapper;

        public VehicleService(
            IVehiclesRepository vehicleRepository,
            IEmployeesRepository employeeRepository,
            ILoggerFactory loggerFactory,
            IMapper mapper
        )
        {
            _logger = loggerFactory.CreateLogger<VehicleService>();
            _vehicleRepository = vehicleRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Метод получения транспорта по его ID
        /// </summary>
        /// <param name="vehicleId">ID транспорта</param>
        /// <returns></returns>
        public async Task<VehicleDTO?> GetByIdAsync(int vehicleId) 
        {
            _logger.LogInformation($"Get vehicle by id, vehicleId - {vehicleId}");
            var vehicle = await _vehicleRepository.GetIdAsync(vehicleId);

            _logger.LogInformation("Return mapping object Vehicle to VehicleDTO");
            return _mapper.Map<VehicleDTO>(vehicle);
        }

        /// <summary>
        /// Метод получения полной коллекции транспорта
        /// </summary>
        /// <returns></returns>
        public IEnumerable<VehicleDTO> GetAll()
        {
            _logger.LogInformation("Getting collection vehicle on the repository");
            var vehicle = _vehicleRepository.All();

            _logger.LogInformation("Return mapping object IEnumerable<Vehicle> to IEnumerable<VehicleDTO>");
            return _mapper.Map<IEnumerable<VehicleDTO>>(vehicle);
        }

        /// <summary>
        /// Метод получения транспорта по ID сотрудника
        /// </summary>
        /// <param name="employeeId">ID сотрудника</param>
        /// <returns></returns>
        public IEnumerable<VehicleDTO> GetByEmployeeId(int employeeId)
        {
            _logger.LogInformation($"Getting collection vehicle on the employeeId, employeeId - {employeeId}");

            var vehicles = _vehicleRepository.GetByEmployeeId(employeeId);

            _logger.LogInformation("Return mapping object IEnumerable<Vehicle> to IEnumerable<VehicleDTO>");
            return _mapper.Map<IEnumerable<VehicleDTO>>(vehicles);
        }

        /// <summary>
        /// Метод добавления транспорта в БД
        /// </summary>
        /// <param name="vehicle">Модель представления транспорта</param>
        /// <returns></returns>
        public async Task<VehicleDTO?> AddAsync(AddVehicleDTO vehicle)
        {
            _logger.LogInformation("Adding new vehicle to the repository");
            var mappedVehicle = _mapper.Map<Vehicle>(vehicle);
            var newVehicle = await _vehicleRepository.AddAsync(mappedVehicle);

            _logger.LogInformation($"The new vehicle, vehicleId - {newVehicle.Id}, numberPlate - {newVehicle.NumberPlate}");
            _logger.LogInformation("Return mapping object Vehicle to VehicleDTO");
            return _mapper.Map<VehicleDTO>(newVehicle);
        }

        /// <summary>
        /// Метод обновления транспорта
        /// </summary>
        /// <param name="vehicle">Модель представления транспорта</param>
        /// <returns></returns>
        public async Task<VehicleDTO?> UpdateAsync(UpdateVehicleDTO vehicle)
        {
            _logger.LogInformation($"Update vehicle on the repository, vehicleId - {vehicle.Id}");
            var mappedVehicle = _mapper.Map<Vehicle>(vehicle);
            var updatedVehicle = await _vehicleRepository.UpdateAsync(mappedVehicle);
            _logger.LogInformation($"New updated vehicle, vehicleId - {updatedVehicle.Id}, numberPlate - {updatedVehicle.NumberPlate}");
            _logger.LogInformation("Return mapping object Vehicle to VehicleDTO");
            return _mapper.Map<VehicleDTO>(updatedVehicle);
        }

        /// <summary>
        /// Метод удаления транспорта
        /// </summary>
        /// <param name="id">ID транспорта</param>
        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation($"Deleting vehicle on the id - {id}");
            await _vehicleRepository.DeleteAsync(t => t.Id == id);
        }

        /// <summary>
        /// Метод проверки номера
        /// </summary>
        /// <param name="numberPlate">Номер</param>
        /// <param name="employeeId">ID сотрудника</param>
        /// <returns></returns>
        public async Task<bool> CheckNumberPlateAsync(string numberPlate, int employeeId)
        {
            _logger.LogInformation($"Checking number plate, numberPlate - {numberPlate}, employeeId - {employeeId}");

            // Находим транспорт по его номерному знаку
            var vehicle = _vehicleRepository.Get(t => t.NumberPlate == numberPlate);

            _logger.LogInformation($"Getting vehicle by the numberPlate, vehicleId - {vehicle?.Id}");
            // Если такого транспорта нету или привязанный владелец не подходящий
            if (vehicle == null || vehicle.EmployeeId != employeeId)
            {
                _logger.LogInformation($"Getted vehicle is a null");
                return false;
            }

            _logger.LogInformation($"Getted employee on the vehicle");
            // Ищем подходящего владельца по полю EmployeeId
            var employee = await _employeeRepository.GetIdAsync(employeeId);

            // Если владелец существует возвращаем true, иначе false
            return employee != null;
        }
    }
}
