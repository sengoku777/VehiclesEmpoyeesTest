using Cars_Test.DTO;
using Cars_Test.DTO.Employee;

namespace Cars_Test.Services.Base
{
    public interface IEmployeeService
    {
        IEnumerable<EmployeeDTO> GetAll();
        Task<EmployeeDTO?> GetByIdAsync(int employeeId);
        EmployeeDTO? Add(AddEmployeeDTO employeeDto);
        Task<EmployeeDTO?> UpdateAsync(UpdateEmployeeDTO updatedEmployeeDto);
        //Task<IEnumerable<VehicleDTO>?> AddVehicleAnEmployee(IEnumerable<int> vehicleIds, int employeeId);
        Task<EmployeeDTO?> AddVehicleAnEmployee(AddVehicleAnEmployeeDTO vehicle);
        Task<DeleteEmployeeDTO> DeleteAsync(int employeeId);
    }
}
