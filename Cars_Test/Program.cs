using Microsoft.EntityFrameworkCore;
using Cars_Test.Services.Base;
using Cars_Test.Services;
using Cars_Test.Data.Repository;
using Cars_Test.SeedData.Base;
using Cars_Test.SeedData;
using Cars_Test.Data.Repository.Contracts;
using Cars_Test.DTO.Profiles;
using Microsoft.OpenApi.Models;

namespace Cars_Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var mappingProfile = typeof(MappingProfile);

            // получаем строку подключения из файла конфигурации
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // добавляем контекст ApplicationContext в качестве сервиса в приложение
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Scoped);

            // --- Services ---
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddScoped<IVehicleService, VehicleService>();

            // --- Seeders ---
            builder.Services.AddScoped<IDBSeeder, EmployeesSeeder>();
            builder.Services.AddScoped<IDBSeeder, VehiclesSeeder>();

            // --- Data bases ---
            builder.Services.AddScoped<IEmployeesRepository, EmployeeRepository>();
            builder.Services.AddScoped<IVehiclesRepository, VehicleRepository>();

            builder.Services.AddAutoMapper(mappingProfile);

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "VehiclesEmployeesTest API", Version = "v1" });
            });

            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AllowNullCollections = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Seed();

            app.Run();
        }
    }
}
