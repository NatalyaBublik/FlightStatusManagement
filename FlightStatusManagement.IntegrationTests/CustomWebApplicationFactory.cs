using FlightStatusManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FlightStatusManagement.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private string _dbName = null!;
        private string _connectionString = null!;

        private const string _server = @"(localdb)\MSSQLLocalDB";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
                services.RemoveAll(typeof(ApplicationDbContext));

                _dbName = $"FlightStatus_Test_{Guid.NewGuid():N}";
                _connectionString =
                    $@"Server={_server};Database={_dbName};Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(_connectionString));
            });
        }

        public async Task InitializeAsync()
        {
            using var scope = Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await db.Database.EnsureCreatedAsync();
        }

        public async Task DisposeAsync()
        {
            try
            {
                using var scope = Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await db.Database.EnsureDeletedAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"[TEST ERROR] {ex}");
            }
        }

        public string GetConnectionString() => _connectionString;
    }
}
