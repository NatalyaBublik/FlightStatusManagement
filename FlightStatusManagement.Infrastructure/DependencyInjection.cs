using FlightStatusManagement.Application.Common.Interfaces;
using FlightStatusManagement.Infrastructure.Auth;
using FlightStatusManagement.Infrastructure.Data;
using FlightStatusManagement.Infrastructure.Data.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FlightStatusManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Db"));

                var audit = sp.GetRequiredService<AuditSaveChangesInterceptor>();
                options.AddInterceptors(audit);
            });

            services.AddScoped<IApplicationDbContext,ApplicationDbContext>();

            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

            services.AddScoped<IPasswordHasher, PasswordHasherService>();
            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<AuditSaveChangesInterceptor>();

            return services;
        }
    }
}
