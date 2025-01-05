using Cars_Test.Data.Entities;
using Cars_Test.DTO;
using Cars_Test.DTO.Vehicle;

namespace Cars_Test.Services.Base
{
    public interface IVehicleService
    {
        Task<VehicleDTO?> GetByIdAsync(int vehicleId);
        IEnumerable<VehicleDTO> GetAll();
        IEnumerable<VehicleDTO> GetByEmployeeId(int employeeId);
        Task<VehicleDTO?> AddAsync(AddVehicleDTO vehicle);
        Task<VehicleDTO?> UpdateAsync(UpdateVehicleDTO vehicle);
        Task DeleteAsync(int id);
        Task<bool> CheckNumberPlateAsync(string numberPlate, int employeeId);
    }
}
