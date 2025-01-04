using Cars_Test.Data.Entities;
using Cars_Test.Data.Repository.Base;
using Cars_Test.Data.Repository.Contracts;
using Cars_Test.SeedData.Base;
using Microsoft.EntityFrameworkCore;

namespace Cars_Test.SeedData
{
    public class EmployeesSeeder : IDBSeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmployeesRepository _repository;

        public EmployeesSeeder(
            ApplicationDbContext context,
            IEmployeesRepository repository
        )
        {
            _context = context;
            _repository = repository;
        }


        public void Seed()
        {
            var subs = _repository.All();

            if (subs.Any())
            {
                return;
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                var items = new[]
                {
                    new Employee { Id = 1, Fullname = "Иван Петров Иванович", Phone = "+79595959595", Position = "Разработчик" },
                    new Employee { Id = 2, Fullname = "Глеб Финов Петрович", Phone = "+795999782295", Position = "Дизайнер" },
                    new Employee { Id = 3, Fullname = "Александр Романов Иванович", Phone = "+997775566667", Position = "Дизайнер" },
                };

                _context.Employees.AddRange(items);
                _context.Database.ExecuteSqlRaw(@"SET IDENTITY_INSERT Employees ON");
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw(@"SET IDENTITY_INSERT Employees OFF");
                transaction.Commit();
            }
        }
    }
}
