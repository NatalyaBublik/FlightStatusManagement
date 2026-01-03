using FlightStatusManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlightStatusManagement.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Role> Roles { get; }
        DbSet<Flight> Flights { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
