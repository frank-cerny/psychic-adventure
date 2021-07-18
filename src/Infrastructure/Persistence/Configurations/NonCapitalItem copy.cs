using bike_selling_app.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bike_selling_app.Infrastructure.Persistence.Configurations
{
    public class RevenueItemConfiguration : IEntityTypeConfiguration<RevenueItem>
    {
        public void Configure(EntityTypeBuilder<RevenueItem> builder)
        {
            builder.Property(ri => ri.Name).HasMaxLength(100).IsRequired();
            builder.Property(ri => ri.Description).HasMaxLength(250).IsRequired();
            builder.Property(ri => ri.SalePrice).IsRequired();
            builder.Property(ri => ri.PlatformSoldOn).HasMaxLength(100).IsRequired();
            builder.Property(ri => ri.ItemType).HasMaxLength(100).IsRequired();
        }
    }
}