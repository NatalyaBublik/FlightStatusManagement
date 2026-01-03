using FlightStatusManagement.Application.Common.Interfaces;
using FlightStatusManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Serilog;

namespace FlightStatusManagement.Infrastructure.Data.Interceptors
{
    public class AuditSaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly ICurrentUserService _currentUser;

        public AuditSaveChangesInterceptor(ICurrentUserService currentUser)
        {
            _currentUser = currentUser;
        }

        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            LogChanges(eventData.Context);

            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            LogChanges(eventData.Context);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void LogChanges(DbContext? context)
        {
            if (context is null) return;

            var username = _currentUser.Username ?? "anonymous";

            var entries = context.ChangeTracker.Entries()
                .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
                .ToList();

            if (entries.Count == 0) return;

            foreach (var e in entries)
            {
                var entityName = e.Entity.GetType().Name;
                var key = TryGetKey(e);

                var changedProps = e.State == EntityState.Modified
                    ? e.Properties.Where(p => p.IsModified && p.Metadata.Name != nameof(User.PasswordHash))
                                  .Select(p => p.Metadata.Name)
                                  .ToArray()
                    : Array.Empty<string>();

                Log.Information(
                    "DB Change. User={User} Entity={Entity} Key={Key} State={State} ChangedProperties={ChangedProps}",
                    username,
                    entityName,
                    key,
                    e.State.ToString(),
                    changedProps.Length == 0 ? "-" : string.Join(",", changedProps)
                );
            }
        }

        private static string? TryGetKey(EntityEntry entry)
        {
            var pk = entry.Metadata.FindPrimaryKey();

            if (pk is null)
            {
                return null;
            }

            var parts = pk.Properties
                .Select(p => entry.Property(p.Name).CurrentValue?.ToString())
                .Where(v => v is not null);

            return string.Join(",", parts!);
        }
    }
}
