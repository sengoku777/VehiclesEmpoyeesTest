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
                return employees == null ? throw new ApplicationException("Error getting fetch employees") : Ok(employees);
            }
            catch (Exception)
            {
                return BadRequest("Internal Server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            try
            {
                var employee = await _employeeService.GetByIdAsync(id);
                return employee == null ? throw new ApplicationException("Error getting fetch employee") : Ok(employee);
            }
            catch (Exception)
            {
                return BadRequest("Internal Server error");
            }
        }

        [HttpPost]
        public IActionResult AddEmployee([FromBody] AddEmployeeDTO addEmployeeDTO)
        {
            try
            {
                var createdEmployee = _employeeService.Add(addEmployeeDTO);
                return createdEmployee == null ? throw new ApplicationException("Error added employee to database") : Ok(createdEmployee);
            }
            catch (Exception)
            {
                return BadRequest("Internal Server error");
            }
        }

        [HttpPost("AddVehicle")]
        public async Task<IActionResult> AddVehicle([FromBody] AddVehicleAnEmployeeDTO addVehicleDTO)
        {
            try
            {
                var employee = await _employeeService.AddVehicleAnEmployee(addVehicleDTO);
                return employee == null ? throw new ApplicationException("Error added vehicle for employee to database") : Ok(employee);
            }
            catch (Exception)
            {
                return BadRequest("Internal Server error");
            }
        }

        //[HttpPost("AddVehicleCollection")]
        //public async Task<IActionResult> AddVehicleCollection([FromBody] IEnumerable<int> addVehicleIdsDTO, int employeeId)
        //{
        //    try
        //    {
        //        var employee = await _employeeService.AddVehicleAnEmployee(addVehicleIdsDTO, employeeId);
        //        return employee == null ? throw new ApplicationException("Error added vehicle for employee to database") : Ok(employee);
        //    }
        //    catch (Exception)
        //    {
        //        return BadRequest("Internal Server error");
        //    }
        //}

        [HttpPut]
        public async Task<IActionResult> UpdateEmployee([FromBody] UpdateEmployeeDTO updateEmployeeDTO)
        {
            try
            {
                var updatedEmployee = await _employeeService.UpdateAsync(updateEmployeeDTO);
                return updatedEmployee == null ? throw new ApplicationException("Error updated employee in the database") : Ok(updatedEmployee);
            }
            catch (Exception)
            {
                return BadRequest("Internal Server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var deletedEmployee = await _employeeService.DeleteAsync(id);
                return deletedEmployee == null ? throw new ApplicationException("Error delete employee in the database") : Ok(deletedEmployee);
            }
            catch (Exception)
            {
                return BadRequest("Internal Server error");
            }
        }

    }
}
