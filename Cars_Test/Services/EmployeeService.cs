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
            var allEmployees = _employeeRepository.All();
            return _mapper.Map<IEnumerable<EmployeeDTO>>(allEmployees);
        }

        /// <summary>
        /// Метод получения сотрудника по его ID
        /// </summary>
        /// <param name="employeeId">ID сотрудника</param>
        /// <returns></returns>
        public async Task<EmployeeDTO?> GetByIdAsync(int employeeId)
        {
            var employeeById = await _employeeRepository.GetIdAsync(employeeId);
            return _mapper.Map<EmployeeDTO>(employeeById) ?? null;
        }

        /// <summary>
        /// Метод добавления сотрудника
        /// </summary>
        /// <param name="employeeDto">Модель представление сотрудника</param>
        /// <returns></returns>
        public async Task<EmployeeDTO?> AddAsync(AddEmployeeDTO employeeDto)
        {
            var originalEmployee = _mapper.Map<Employee>(employeeDto);
            var newEmployee = await _employeeRepository.AddAsync(originalEmployee);
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
            var employee = await _employeeRepository.GetIdAsync(updatedEmployeeDto.Id)
                ?? throw new Exception("Employee doesn't exist");

            // Делаем маппинг модель типа UpdateEmployeeDTO в Employee
            _mapper.Map(updatedEmployeeDto, employee);

            // Обновляем данные в БД
            var updatedEmployee = await _employeeRepository.UpdateAsync(employee);

            // Метод добавления транспорта к сотруднику
            await LinkVehicleAsync(updatedEmployeeDto.VehicleIds, updatedEmployee);

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
            var employee = await _employeeRepository.GetIdAsync(employeeId)
                ?? throw new Exception("Employee doesn't exist");

            await _employeeRepository.DeleteAsync(t => t.Id == employee.Id);

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
            if (vehicle == null) 
            { 
                throw new Exception("Vehicle is a null"); 
            }

            var employee = await _employeeRepository.GetIdAsync(vehicle.EmployeeId) 
                ?? throw new Exception("Employee doesn't exist");

            var existedVehicle = await _vehicleRepository.GetIdAsync(vehicle.EmployeeId) 
                ?? throw new Exception("Vehicle doesn't exist");

            existedVehicle.EmployeeId = vehicle.EmployeeId;
            await _vehicleRepository.UpdateAsync(existedVehicle);

            return _mapper.Map<EmployeeDTO>(employee);
        }


        //public async Task<IEnumerable<VehicleDTO>> AddVehicleAnEmployee(IEnumerable<int> vehicleIds, int employeeId)
        //{
        //    if (vehicleIds == null || vehicleIds?.Count() == 0) { throw new Exception("Vehicle is a null or empty"); }

        //    var employee = await _employeeRepository.GetIdAsync(employeeId) ?? throw new Exception("Employee doesn't exist");

        //    var addedCars = new List<VehicleDTO>();

        //    foreach (var id in vehicleIds)
        //    {
        //        var existedVehicle = await _vehicleRepository.GetIdAsync(id);

        //        if (existedVehicle == null)
        //        {
        //            var mappedVehicle = _mapper.Map<Vehicle>(existedVehicle);
        //            var newVehicle = _vehicleRepository.Add(mappedVehicle);
        //            addedCars.Add(_mapper.Map<VehicleDTO>(newVehicle));
        //            continue;
        //        }

        //        existedVehicle.EmployeeId = employeeId;
        //        _vehicleRepository.Update(existedVehicle);
        //    }

        //    return addedCars;
        //}
    }
}
