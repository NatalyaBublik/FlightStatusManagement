using FlightStatusManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightStatusManagement.Infrastructure.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                   .HasMaxLength(256)
                   .IsRequired();

            builder.HasIndex(x => x.Code)
                   .IsUnique();
        }
    }
}
