using bike_selling_app.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bike_selling_app.Infrastructure.Persistence.Configurations
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable("Items");
            builder.HasDiscriminator(i => i.ItemType)
            .HasValue<NonCapitalItem>("non-capital")
            .HasValue<CapitalItem>("capital")
            .HasValue<RevenueItem>("revenue")
            .HasValue<ExpenseItem>("expense");
        }
    }
}