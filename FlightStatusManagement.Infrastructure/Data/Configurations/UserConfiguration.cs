using FlightStatusManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlightStatusManagement.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Username)
                   .HasMaxLength(256)
                   .IsRequired();

            builder.HasIndex(x => x.Username)
                   .IsUnique();

            builder.Property(x => x.PasswordHash)
                   .HasMaxLength(256)
                   .IsRequired();

            builder.HasOne(x => x.Role)
                   .WithMany()
                   .HasForeignKey(x => x.RoleId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
