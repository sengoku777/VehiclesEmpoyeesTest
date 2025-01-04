using Cars_Test.Data.Entities;
using Cars_Test.DTO;
using Cars_Test.DTO.Vehicle;

namespace Cars_Test.Services.Base
{
    public interface IVehicleService
    {
        Task<VehicleDTO> GetByIdAsync(int vehicleId);
        IEnumerable<VehicleDTO> GetAll();
        IEnumerable<VehicleDTO> GetByEmployeeId(int employeeId);
        VehicleDTO Add(AddVehicleDTO vehicle);
        VehicleDTO Update(UpdateVehicleDTO vehicle);
        void Delete(int id);
        bool CheckNumberPlate(string numberPlate, int employeeId);
    }
}
