using Cars_Test.Data.Entities;
using Cars_Test.Data.Repository.Contracts;
using Cars_Test.SeedData.Base;
using Microsoft.EntityFrameworkCore;

namespace Cars_Test.SeedData
{
    public class VehiclesSeeder : IDBSeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly IVehiclesRepository _repository;

        public VehiclesSeeder(
            ApplicationDbContext context,
            IVehiclesRepository repository
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
                    new Vehicle { Id = 1, Brand = "Mercedes-Benz", Color = "Черный", Model = "AMG E60", NumberPlate = "X777XX777", EmployeeId = 1 },
                    new Vehicle { Id = 2, Brand = "BMW", Color = "Серо-зеленый", Model = "E39", NumberPlate = "E741РX777", EmployeeId = 2 },
                    new Vehicle { Id = 3, Brand = "Rolls-Royce", Color = "Белый", Model = "Phantom", NumberPlate = "В123ОР777", EmployeeId = 3  },
                };

                _context.Vehicles.AddRange(items);
                _context.Database.ExecuteSqlRaw(@"SET IDENTITY_INSERT Vehicles ON");
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw(@"SET IDENTITY_INSERT Vehicles OFF");
                transaction.Commit();
            }
        }
    }
}
