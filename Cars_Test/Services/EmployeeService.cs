using AutoMapper;
using Cars_Test.Data.Entities;
using Cars_Test.Data.Repository.Contracts;
using Cars_Test.DTO;
using Cars_Test.DTO.Employee;
using Cars_Test.Services.Base;

namespace Cars_Test.Services
{
    /// <summary>
    /// Сервис для работы с данными сотрудников
    /// </summary>
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeesRepository _employeeRepository;
        private readonly IVehiclesRepository _vehicleRepository;

        private readonly ILogger<EmployeeService> _logger;
        private readonly IMapper _mapper;

        public EmployeeService(
            IEmployeesRepository employeeRepository,
            IVehiclesRepository vehicleRepository,
            ILoggerFactory loggerFactory,
            IMapper mapper
        )
        {
            _logger = loggerFactory.CreateLogger<EmployeeService>();
            _employeeRepository = employeeRepository;
            _vehicleRepository = vehicleRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Метод получения коллекции всех сотрудников
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EmployeeDTO> GetAll()
        {
            _logger.LogInformation("Get employees collection");
            var allEmployees = _employeeRepository.All();
            
            _logger.LogInformation("Mapping and return collection type IEnumerable<EmployeeDTO>");
            return _mapper.Map<IEnumerable<EmployeeDTO>>(allEmployees);
        }

        /// <summary>
        /// Метод получения сотрудника по его ID
        /// </summary>
        /// <param name="employeeId">ID сотрудника</param>
        /// <returns></returns>
        public async Task<EmployeeDTO?> GetByIdAsync(int employeeId)
        {
            _logger.LogInformation($"Get employee by ID - {employeeId}");
            var employeeById = await _employeeRepository.GetIdAsync(employeeId);

            _logger.LogInformation("Mapping and return object type EmployeeDTO");
            return _mapper.Map<EmployeeDTO>(employeeById) ?? null;
        }

        /// <summary>
        /// Метод добавления сотрудника
        /// </summary>
        /// <param name="employeeDto">Модель представление сотрудника</param>
        /// <returns></returns>
        public async Task<EmployeeDTO?> AddAsync(AddEmployeeDTO employeeDto)
        {
            _logger.LogInformation("Add employee");
            _logger.LogInformation("Mapping AddEmployeeDTO to Employee");
            var originalEmployee = _mapper.Map<Employee>(employeeDto);

            _logger.LogInformation("Added employee to repository and return the added employee");
            var newEmployee = await _employeeRepository.AddAsync(originalEmployee);

            _logger.LogInformation("Mapping Employee to AddEmployeeDTO");
            return _mapper.Map<EmployeeDTO>(newEmployee);
        }

        /// <summary>
        /// Метод обновления данных сотрудника
        /// </summary>
        /// <param name="updatedEmployeeDto">Модель представления сотрудника</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<EmployeeDTO?> UpdateAsync(UpdateEmployeeDTO updatedEmployeeDto)
        {
            _logger.LogInformation($"Updating employee - {updatedEmployeeDto}");

            var employee = await _employeeRepository.GetIdAsync(updatedEmployeeDto.Id)
                ?? throw new Exception("Employee doesn't exist");

            _logger.LogInformation($"Getting employee on the repository, employeeId - {employee.Id}");

            // Делаем маппинг модель типа UpdateEmployeeDTO в Employee
            _logger.LogInformation("Mapping UpdateEmployeeDTO to Employee");
            _mapper.Map(updatedEmployeeDto, employee);

            // Обновляем данные в БД
            _logger.LogInformation($"Updating employee on the repository");
            var updatedEmployee = await _employeeRepository.UpdateAsync(employee);

            // Метод добавления транспорта к сотруднику
            _logger.LogInformation($"Linked updated vehicle employee on the repository");
            await LinkVehicleAsync(updatedEmployeeDto.VehicleIds, updatedEmployee);

            _logger.LogInformation("Mapping Employee to EmployeeDTO");
            return _mapper.Map<EmployeeDTO>(employee);
        }

        /// <summary>
        /// Метод привязки транспорта к конкретному сотруднику
        /// </summary>
        /// <param name="vehicleIds">Коллекция ID транспорта </param>
        /// <param name="addedEmployee">Модель сотрудника</param>
        /// <returns></returns>
        private async Task LinkVehicleAsync(ICollection<int> vehicleIds, Employee? addedEmployee)
        {
            if (vehicleIds == null || !vehicleIds.Any() || addedEmployee == null)
                return;

            // Перебираем ID транспорта, дабы не занести пользователю не существующий транспорт
            foreach (var id in vehicleIds)
            {
                var vehicle = await _vehicleRepository.GetIdAsync(id);

                if(vehicle != null)
                {
                    vehicle.EmployeeId = addedEmployee.Id;
                    await _vehicleRepository.UpdateAsync(vehicle);
                }
            }
        }

        /// <summary>
        /// Метод удаления сотрудника
        /// </summary>
        /// <param name="employeeId">ID сотрудника</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<DeleteEmployeeDTO> DeleteAsync(int employeeId)
        {
            _logger.LogInformation($"Delete employee, employeeId - {employeeId}");

            var employee = await _employeeRepository.GetIdAsync(employeeId)
                ?? throw new Exception("Employee doesn't exist");

            _logger.LogInformation($"Delete employee on the repository");
            await _employeeRepository.DeleteAsync(t => t.Id == employee.Id);

            _logger.LogInformation("Mapping Employee to DeleteEmployeeDTO");
            return _mapper.Map<DeleteEmployeeDTO>(employee);
        }

        /// <summary>
        /// Метод добавления транспорта сотруднику 
        /// </summary>
        /// <param name="vehicle">Модель представления траснпорта</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<EmployeeDTO?> AddVehicleAnEmployee(AddVehicleAnEmployeeDTO vehicle)
        {
            _logger.LogInformation($"Add vehicle to employee, employeeId - {vehicle.EmployeeId}, vehicleId - {vehicle.VehicleId}");

            if (vehicle == null) 
            { 
                throw new Exception("Vehicle is a null"); 
            }

            var employee = await _employeeRepository.GetIdAsync(vehicle.EmployeeId) 
                ?? throw new Exception("Employee doesn't exist");

            var existedVehicle = await _vehicleRepository.GetIdAsync(vehicle.EmployeeId) 
                ?? throw new Exception("Vehicle doesn't exist");

            existedVehicle.EmployeeId = vehicle.EmployeeId;
            _logger.LogInformation("Add vehicle to employee on the repository");
            await _vehicleRepository.UpdateAsync(existedVehicle);

            _logger.LogInformation("Mapping Employee to EmployeeDTO");
            return _mapper.Map<EmployeeDTO>(employee);
        }
    }
}
