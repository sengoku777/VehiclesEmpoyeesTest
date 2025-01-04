using Cars_Test.SeedData.Base;

namespace Cars_Test.SeedData
{
    public static class ApplicationScopeProvider
    {
        public static void Seed(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                        .GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var seeders = serviceScope.ServiceProvider.GetServices<IDBSeeder>();

                foreach (var seeder in seeders)
                {
                    seeder.Seed();
                }
            }
        }
    }
}
