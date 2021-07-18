using bike_selling_app.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bike_selling_app.Infrastructure.Persistence.Configurations
{
    public class CapitalItemConfiguration : IEntityTypeConfiguration<CapitalItem>
    {
        public void Configure(EntityTypeBuilder<CapitalItem> builder)
        {
            builder.Property(ci => ci.Cost).IsRequired();
            builder.Property(ci => ci.Name).HasMaxLength(100).IsRequired();
            builder.Property(ci => ci.Description).HasMaxLength(250).IsRequired();
        }
    }
}