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
        public async Task<IActionResult> CheckNumberPlate(string numberPlate, int employeeId)
        {
            try
            {
                var isValidateNumberplate = await _vehicleService.CheckNumberPlateAsync(numberPlate, employeeId);
                return Ok(isValidateNumberplate);
            }
            catch (Exception)
            {
                return BadRequest("Error check number plate");
            }
        }

        [HttpGet]
        public IActionResult GetAllVehicles()
        {
            try
            {
                var vehicles = _vehicleService.GetAll();
                return vehicles == null ? throw new ApplicationException("Error get vehicles in the database") : Ok(vehicles);
            }
            catch (Exception)
            {
                return BadRequest("Error on the get collection vehicle");
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
                return BadRequest("Erorr on the getting vehicle");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddVehicle([FromBody] AddVehicleDTO createVehicleDto)
        {
            try
            {
                var vehicle = await _vehicleService.AddAsync(createVehicleDto);
                return vehicle == null ? throw new ApplicationException("Error added vehicle in the database") : Ok(vehicle);
            }
            catch (Exception)
            {
                return BadRequest("Erorr on the adding vehicle");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateVehicle([FromBody] UpdateVehicleDTO updateVehicleDto)
        {
            try
            {
                var vehicle = await _vehicleService.UpdateAsync(updateVehicleDto);
                return vehicle == null ? throw new ApplicationException("Error updating vehicle in the database") : Ok(vehicle);
            }
            catch (Exception)
            {
                return BadRequest("Erorr on the updating vehicle");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            try
            {
                await _vehicleService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Erorr on the deleting vehicle");
            }
        }
    }
}
