using Cars_Test.Data.Entities;
using Cars_Test.Data.Repository.Base;
using Cars_Test.Data.Repository.Contracts;

namespace Cars_Test.Data.Repository
{
    public class VehicleRepository
    : BaseRepository<Vehicle, ApplicationDbContext>, IVehiclesRepository
    {
        public VehicleRepository(
            ApplicationDbContext context,
            ILogger<Vehicle> logger
        ) : base(context, logger)
        {
        }

        public IEnumerable<Vehicle> GetByEmployeeId(int employeeId)
        {
            return _context.Vehicles
                .Where(t => t.EmployeeId == employeeId)
                .ToList();
        }

        public async Task<Vehicle?> GetIdAsync(int vehicleId)
        {
            return await _context.Vehicles.FindAsync(vehicleId);
        }
    }
}
