using FlightStatusManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FlightStatusManagement.IntegrationTests
{
    public static class TestDatabaseHelper
    {
        public static async Task ResetDatabaseAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            db.Flights.RemoveRange(db.Flights);

            await db.SaveChangesAsync();
        }
    }
}
