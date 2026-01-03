using FlightStatusManagement.Application.Common.Interfaces;
using FlightStatusManagement.Infrastructure.Auth;
using FlightStatusManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FlightStatusManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Db"));
            });

            services.AddScoped<IApplicationDbContext,ApplicationDbContext>();

            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

            services.AddScoped<IPasswordHasher, PasswordHasherService>();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
