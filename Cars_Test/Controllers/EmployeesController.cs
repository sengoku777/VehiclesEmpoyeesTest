using Cars_Test.DTO.Employee;
using Cars_Test.Services.Base;
using Microsoft.AspNetCore.Mvc;

namespace Cars_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var employees = _employeeService.GetAll();
                return employees == null ? NotFound("Error getting fetch employees") : Ok(employees);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Error on the get collection employees");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            try
            {
                var employee = await _employeeService.GetByIdAsync(id);
                return employee == null ? NotFound("Error getting fetch employee") : Ok(employee);
            }
            catch (Exception)
            {
                return BadRequest("Error on the get employee");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] AddEmployeeDTO addEmployeeDTO)
        {
            try
            {
                var addedEmployee = await _employeeService.AddAsync(addEmployeeDTO);
                return addedEmployee == null ? NotFound("Error added employee to database") : Ok(addedEmployee);
            }
            catch (Exception)
            {
                return BadRequest("Error on the adding employee");
            }
        }

        [HttpPost("AddVehicle")]
        public async Task<IActionResult> AddVehicle([FromBody] AddVehicleAnEmployeeDTO addVehicleDTO)
        {
            try
            {
                var employee = await _employeeService.AddVehicleAnEmployee(addVehicleDTO);
                return employee == null ? NotFound("Error added vehicle for employee to database") : Ok(employee);
            }
            catch (Exception)
            {
                return BadRequest("Error on the adding vehicle to the employee");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmployee([FromBody] UpdateEmployeeDTO updateEmployeeDTO)
        {
            try
            {
                var updatedEmployee = await _employeeService.UpdateAsync(updateEmployeeDTO);
                return updatedEmployee == null ? NotFound("Error updated employee in the database") : Ok(updatedEmployee);
            }
            catch (Exception)
            {
                return BadRequest("Error on the updating employee");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var deletedEmployee = await _employeeService.DeleteAsync(id);
                return deletedEmployee == null ? NotFound("Error delete employee in the database") : Ok(deletedEmployee);
            }
            catch (Exception)
            {
                return BadRequest("Error on the deleting employee");
            }
        }

    }
}
