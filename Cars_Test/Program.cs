using Microsoft.EntityFrameworkCore;
using Cars_Test.Services.Base;
using Cars_Test.Services;
using Cars_Test.Data.Repository;
using Cars_Test.SeedData.Base;
using Cars_Test.SeedData;
using Cars_Test.Data.Repository.Contracts;
using Cars_Test.DTO.Profiles;
using Microsoft.OpenApi.Models;
using NLog.Web;
using NLog;

namespace Cars_Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Early init of NLog to allow startup and exception logging, before host is built
            var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
            logger.Debug("Init main");

            try
            {
                var builder = WebApplication.CreateBuilder(args);

                var mappingProfile = typeof(MappingProfile);

                // �������� ������ ����������� �� ����� ������������
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

                // ��������� �������� ApplicationContext � �������� ������� � ����������
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

                // NLog: Setup NLog for Dependency injection
                builder.Logging.ClearProviders();
                builder.Host.UseNLog();

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
            catch (Exception exception)
            {
                // NLog: catch setup errors
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                LogManager.Shutdown();
            }
        }
    }
}
