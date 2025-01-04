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
        public async Task<VehicleDTO> GetByIdAsync(int vehicleId) 
        {
            var vehicle = await _vehicleRepository.GetIdAsync(vehicleId);
            return _mapper.Map<VehicleDTO>(vehicle);
        }

        /// <summary>
        /// Метод получения полной коллекции транспорта
        /// </summary>
        /// <returns></returns>
        public IEnumerable<VehicleDTO> GetAll()
        {
            var vehicle = _vehicleRepository.All();
            return _mapper.Map<IEnumerable<VehicleDTO>>(vehicle);
        }

        /// <summary>
        /// Метод получения транспорта по ID сотрудника
        /// </summary>
        /// <param name="employeeId">ID сотрудника</param>
        /// <returns></returns>
        public IEnumerable<VehicleDTO> GetByEmployeeId(int employeeId) 
        {
            var vehicles = _vehicleRepository.GetByEmployeeId(employeeId);
            return _mapper.Map<IEnumerable<VehicleDTO>>(vehicles);
        }

        /// <summary>
        /// Метод добавления транспорта в БД
        /// </summary>
        /// <param name="vehicle">Модель представления транспорта</param>
        /// <returns></returns>
        public VehicleDTO Add(AddVehicleDTO vehicle) 
        { 
            var mappedVehicle = _mapper.Map<Vehicle>(vehicle);
            var newVehicle = _vehicleRepository.Add(mappedVehicle);
            return _mapper.Map<VehicleDTO>(newVehicle);
        }

        /// <summary>
        /// Метод обновления транспорта
        /// </summary>
        /// <param name="vehicle">Модель представления транспорта</param>
        /// <returns></returns>
        public VehicleDTO Update(UpdateVehicleDTO vehicle)
        {
            var mappedVehicle = _mapper.Map<Vehicle>(vehicle);
            var updatedVehicle = _vehicleRepository.Update(mappedVehicle);
            return _mapper.Map<VehicleDTO>(updatedVehicle);
        }

        /// <summary>
        /// Метод удаления транспорта
        /// </summary>
        /// <param name="id">ID транспорта</param>
        public void Delete(int id) 
        {
            _vehicleRepository.Delete(t => t.Id == id);
        }

        /// <summary>
        /// Метод проверки номера
        /// </summary>
        /// <param name="numberPlate">Номер</param>
        /// <param name="employeeId">ID сотрудника</param>
        /// <returns></returns>
        public bool CheckNumberPlate(string numberPlate, int employeeId) 
        {
            // Находим транспорт по его номерному знаку
            var vehicle = _vehicleRepository.Get(t => t.NumberPlate == numberPlate);

            // Если такого транспорта нету или привязанный владелец не подходящий
            if (vehicle == null || vehicle.EmployeeId != employeeId)
                return false;

            // Ищем подходящего владельца по полю EmployeeId
            var employee = _employeeRepository.GetIdAsync(employeeId);

            // Если владелец существует возвращаем true, иначе false
            return employee != null;
        }
    }
}
