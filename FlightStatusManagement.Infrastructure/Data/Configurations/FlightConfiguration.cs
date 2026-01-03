using FlightStatusManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightStatusManagement.Infrastructure.Data.Configurations
{
    public class FlightConfiguration : IEntityTypeConfiguration<Flight>
    {
        public void Configure(EntityTypeBuilder<Flight> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Origin)
                   .HasMaxLength(256)
                   .IsRequired();

            builder.Property(x => x.Destination)
                   .HasMaxLength(256)
                   .IsRequired();

            builder.Property(x => x.Departure)
                   .IsRequired();

            builder.Property(x => x.Arrival)
                   .IsRequired();

            builder.Property(x => x.Status)
                   .IsRequired();
        }
    }
}
