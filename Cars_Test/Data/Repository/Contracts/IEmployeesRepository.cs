using Cars_Test.Data.Entities;
using Cars_Test.Data.Repository.Base;

namespace Cars_Test.Data.Repository.Contracts
{
    public interface IEmployeesRepository : IDataRepository<Employee>
    {
        Task<Employee> GetIdAsync(int employeeId);
    }
}
