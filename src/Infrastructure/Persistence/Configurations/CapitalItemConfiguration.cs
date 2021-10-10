using bike_selling_app.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bike_selling_app.Infrastructure.Persistence.Configurations
{
    public class CapitalItemConfiguration : IEntityTypeConfiguration<CapitalItem>
    {
        public void Configure(EntityTypeBuilder<CapitalItem> builder)
        {
            // All fields except those extended from the abstract class are nullable (the nature of Table by Hiearchy in EFC)
            builder.Property(ci => ci.Cost).IsRequired();
            builder.Property(ci => ci.Name).HasMaxLength(100).IsRequired();
            builder.Property(ci => ci.Description).HasMaxLength(250).IsRequired();
        }
    }
}