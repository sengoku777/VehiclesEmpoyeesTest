using Cars_Test.Data.Entities;
using Cars_Test.Data.Repository.Base;

namespace Cars_Test.Data.Repository.Contracts
{
    public interface IVehiclesRepository : IDataRepository<Vehicle>
    {
        Task<Vehicle?> GetIdAsync(int vehicleId);

        IEnumerable<Vehicle> GetByEmployeeId(int employeeId);
    }
}
