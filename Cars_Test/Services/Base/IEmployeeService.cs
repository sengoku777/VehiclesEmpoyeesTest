using Cars_Test.DTO;
using Cars_Test.DTO.Employee;

namespace Cars_Test.Services.Base
{
    public interface IEmployeeService
    {
        IEnumerable<EmployeeDTO> GetAll();
        Task<EmployeeDTO?> GetByIdAsync(int employeeId);
        Task<EmployeeDTO?> AddAsync(AddEmployeeDTO employeeDto);
        Task<EmployeeDTO?> UpdateAsync(UpdateEmployeeDTO updatedEmployeeDto);
        Task<EmployeeDTO?> AddVehicleAnEmployee(AddVehicleAnEmployeeDTO vehicle);
        Task<DeleteEmployeeDTO> DeleteAsync(int employeeId);
    }
}
