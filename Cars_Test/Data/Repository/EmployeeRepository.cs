using Cars_Test.Data.Entities;
using Cars_Test.Data.Repository.Base;
using Cars_Test.Data.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Cars_Test.Data.Repository
{
    public class EmployeeRepository
     : BaseRepository<Employee, ApplicationDbContext>, IEmployeesRepository
    {
        public EmployeeRepository(
            ApplicationDbContext context,
            ILogger<Employee> logger
        ) : base(context, logger)
        {
        }

        public override IEnumerable<Employee> All()
            => _context.Employees.Include(e => e.Vehicles).ToList();

        public async Task<Employee> GetIdAsync(int employeeId)
        {
            return await _context.Employees
            .Include(e => e.Vehicles).AsNoTracking()
            .SingleOrDefaultAsync(e => e.Id == employeeId);
        }
    }
}
