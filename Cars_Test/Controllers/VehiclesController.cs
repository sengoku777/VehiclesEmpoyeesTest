using AutoMapper;
using Cars_Test.DTO.Vehicle;
using Cars_Test.Services.Base;
using Microsoft.AspNetCore.Mvc;

namespace Cars_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly IMapper _mapper;

        public VehiclesController(IVehicleService vehicleService, IMapper mapper)
        {
            _vehicleService = vehicleService;
            _mapper = mapper;
        }

        [HttpGet("checkNumberPlate/{numberPlate}/employee/{employeeId}")]
        public IActionResult CheckNumberPlate(string numberPlate, int employeeId)
        {
            try
            {
                var isValidateNumberplate = _vehicleService.CheckNumberPlate(numberPlate, employeeId);
                return Ok(isValidateNumberplate);
            }
            catch (Exception)
            {
                return BadRequest("Internal Server error");
            }
        }

        [HttpGet]
        public IActionResult GetAllVehicles()
        {
            try
            {
                var vehicles = _vehicleService.GetAll();
                return vehicles == null ? throw new ApplicationException("Error delete vehicles in the database") : Ok(vehicles);
            }
            catch (Exception)
            {
                return BadRequest("Internal Server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicleById(int id)
        {
            try
            {
                var vehicle = await _vehicleService.GetByIdAsync(id); 
                return vehicle == null ? throw new ApplicationException("Error getting vehicle in the database") : Ok(vehicle);
            }
            catch (Exception)
            {
                return BadRequest("Internal Server error");
            }
        }

        [HttpPost]
        public IActionResult AddVehicle([FromBody] AddVehicleDTO createVehicleDto)
        {
            try
            {
                var vehicle = _vehicleService.Add(createVehicleDto);
                return vehicle == null ? throw new ApplicationException("Error added vehicle in the database") : Ok(vehicle);
            }
            catch (Exception)
            {
                return BadRequest("Internal Server error");
            }
        }

        [HttpPut]
        public IActionResult UpdateVehicle([FromBody] UpdateVehicleDTO updateVehicleDto)
        {
            try
            {
                var vehicle = _vehicleService.Update(updateVehicleDto);
                return vehicle == null ? throw new ApplicationException("Error updating vehicle in the database") : Ok(vehicle);
            }
            catch (Exception)
            {
                return BadRequest("Internal Server error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteVehicle(int id)
        {
            try
            {
                _vehicleService.Delete(id);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Internal Server error");
            }
        }
    }
}
