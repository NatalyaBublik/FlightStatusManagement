using FlightStatusManagement.Application.Common.Interfaces;
using FlightStatusManagement.Domain.Entities;
using FlightStatusManagement.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace FlightStatusManagement.IntegrationTests
{
    public static class DbSeedExtensions
    {
        public static async Task SeedUsersAsync(this IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

            if (!db.Roles.Any())
            {
                db.Roles.AddRange(
                    new Role { Code = "User" },
                    new Role { Code = "Moderator" }
                );

                await db.SaveChangesAsync();
            }

            if (!db.Users.Any())
            {
                var modRoleId = db.Roles.Single(r => r.Code == "Moderator").Id;
                var userRoleId = db.Roles.Single(r => r.Code == "User").Id;

                db.Users.AddRange(
                    new User { Username = "moderator_user", PasswordHash = hasher.Hash("12345"), RoleId = modRoleId },
                    new User { Username = "user_user", PasswordHash = hasher.Hash("12345"), RoleId = userRoleId }
                );

                await db.SaveChangesAsync();
            }
        }
    }
}
