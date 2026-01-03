using FlightStatusManagement.Domain.Entities;
using FlightStatusManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightStatusManagement.Infrastructure.Seed
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context, ILogger logger, CancellationToken ct = default)
        {
            try
            {
                if ((await context.Database.GetPendingMigrationsAsync(ct)).Any())
                {
                    await context.Database.MigrateAsync(ct);
                }

                if (!await context.Roles.AnyAsync(ct))
                {
                    var roles = new List<Role>
                    {
                        new Role { Code = "User" },
                        new Role { Code = "Moderator" }
                    };

                    await context.Roles.AddRangeAsync(roles);

                    await context.SaveChangesAsync(ct);

                    logger.LogInformation("Seeded roles");
                }

                if (!await context.Users.AnyAsync(ct))
                {
                    var moderatorRoleId = await context.Roles
                        .Where(r => r.Code == "Moderator")
                        .Select(r => r.Id)
                        .SingleAsync(ct);

                    var userRoleId = await context.Roles
                        .Where(r => r.Code == "User")
                        .Select(r => r.Id)
                        .SingleAsync(ct);

                    context.Users.AddRange(
                        new User { Username = "moderator_user", PasswordHash = "TEMP_HASH_MODERATOR", RoleId = moderatorRoleId },
                        new User { Username = "user_user", PasswordHash = "TEMP_HASH_USER", RoleId = userRoleId }
                    );

                    await context.SaveChangesAsync(ct);

                    logger.LogInformation("Seeded users");
                }

                logger.LogInformation("Database seeding completed successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }
    }
}
